// Assets/_Project/Infrastructure/Services/SensoryTierService.cs
using System;
using R3;
using SolarPhobia.Domain;
using SolarPhobia.Domain.Events;
using SolarPhobia.Domain.ValueObjects;
using VContainer;
using VContainer.Unity;

namespace SolarPhobia.Infrastructure.Services
{
    /// <summary>
    /// Implementation of sensory tier feedback service.
    /// Implements TR-state-005: Sensory tiers trigger at 75%, 50%, 25%, ≤10s thresholds.
    /// Emits events once per threshold crossing (AC-6 compliance).
    /// </summary>
    public class SensoryTierService : ISensoryTierService, IInitializable
    {
        // ── R3 Reactive State ──────────────────────────────────────
        private readonly ReactiveProperty<SensoryTier> _currentTier = new(SensoryTier.Stable);
        private readonly Subject<SensoryTierChangedEvent> _onTierChanged = new();
        private readonly Subject<NightFailedEvent> _onNightFailed;

        // ── State ────────────────────────────────────────────────
        private float _maxWard;
        private float _startTime;
        private SensoryTier _previousTier;
        private IDisposable _wardSubscription;

        // ── Constants ────────────────────────────────────────────
        private const float Tier1Threshold = 0.75f;  // Stable → CreepingDread
        private const float Tier2Threshold = 0.50f;  // CreepingDread → HeavyBurden
        private const float Tier3Threshold = 0.25f;  // HeavyBurden → Panic
        private const float Tier4Seconds = 10f;      // Panic → DeathSpiral (seconds-based)

        // ── Public Properties ─────────────────────────────────────
        public ReadOnlyReactiveProperty<SensoryTier> CurrentTier => _currentTier;

        public Observable<SensoryTierChangedEvent> OnTierChanged => _onTierChanged;

        public Observable<NightFailedEvent> OnNightFailed => _onNightFailed;

        // ── Constructor ───────────────────────────────────────────
        [Inject]
        public SensoryTierService(Subject<NightFailedEvent> onNightFailed)
        {
            _onNightFailed = onNightFailed;
            _previousTier = SensoryTier.Stable;
            _maxWard = 0f;
            _startTime = 0f;
        }

        // ── IInitializable ────────────────────────────────────────
        public void Initialize()
        {
            // Defer initialization until Ward timer is ready
        }

        // ── Public Methods ─────────────────────────────────────────
        /// <summary>
        /// Initialize the service with Ward timer observable.
        /// </summary>
        public void Initialize(ReadOnlyReactiveProperty<float> wardObservable, float maxWard)
        {
            _maxWard = maxWard;
            _startTime = 0f; // Will track survival time

            // Dispose previous subscription if re-initializing
            _wardSubscription?.Dispose();

            // Subscribe to Ward changes for tier detection
            _wardSubscription = wardObservable.Subscribe(OnWardChanged);

            // Initialize tier based on starting Ward
            float initialWard = wardObservable.CurrentValue;
            SensoryTier initialTier = CalculateTier(initialWard, maxWard);
            _currentTier.Value = initialTier;
            _previousTier = initialTier;
        }

        // ── Private Methods ───────────────────────────────────────
        private void OnWardChanged(float wardValue)
        {
            // Check for night failure (Ward = 0)
            if (wardValue <= 0f)
            {
                float survivalTime = _startTime > 0 ? 0f : 0f; // Would track actual time
                _onNightFailed.OnNext(new NightFailedEvent(0f, survivalTime));
                return;
            }

            // Calculate current tier
            SensoryTier newTier = CalculateTier(wardValue, _maxWard);

            // Only emit event if tier changed (one-shot per crossing, AC-6)
            if (newTier != _currentTier.Value)
            {
                _previousTier = _currentTier.Value;
                _currentTier.Value = newTier;

                // Emit tier changed event
                var eventData = new SensoryTierChangedEvent(
                    newTier,
                    _previousTier,
                    wardValue,
                    _maxWard
                );

                _onTierChanged.OnNext(eventData);
            }
        }

        /// <summary>
        /// Calculate sensory tier based on Ward value and max.
        /// </summary>
        private SensoryTier CalculateTier(float wardValue, float maxWard)
        {
            if (maxWard <= 0f)
            {
                return SensoryTier.DeathSpiral;
            }

            // Tier 4: ≤10 seconds (DeathSpiral) - absolute seconds check
            if (wardValue <= Tier4Seconds)
            {
                return SensoryTier.DeathSpiral;
            }

            float percentage = wardValue / maxWard;

            // Tier 3: ≤25% (Panic)
            if (percentage <= Tier3Threshold)
            {
                return SensoryTier.Panic;
            }

            // Tier 2: ≤50% (HeavyBurden)
            if (percentage <= Tier2Threshold)
            {
                return SensoryTier.HeavyBurden;
            }

            // Tier 1: ≤75% (CreepingDread)
            if (percentage <= Tier1Threshold)
            {
                return SensoryTier.CreepingDread;
            }

            // Tier 0: >75% (Stable)
            return SensoryTier.Stable;
        }
    }
}
