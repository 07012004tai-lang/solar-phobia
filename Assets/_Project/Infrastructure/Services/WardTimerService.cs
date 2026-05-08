using R3;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;
using UnityEngine;

namespace SolarPhobia.Infrastructure.Services
{
    /// <summary>
    /// Implementation of Ward Timer service.
    /// Manages the Ward timer value and applies costs for player actions.
    /// </summary>
    public class WardTimerService : IWardTimerService
    {
        // ── R3 Reactive State ──────────────────────────────────────
        private readonly ReactiveProperty<float> _currentWard = new();
        private readonly Subject<float> _wardChangedSubject = new();

        // ── Dependencies ────────────────────────────────────────
        private readonly IPhaseStateMachine _phaseStateMachine;

        // ── State ────────────────────────────────────────────────
        private float _maxWard;
        private float _drainRate;

        // ── Constants ────────────────────────────────────────────
        private const float BaseWardSec = 10.0f;
        private const float WardPerGhostSec = 30.0f;
        private const float DefaultDrainRate = 1.0f;

        public float CurrentWard
        {
            get => _currentWard.Value;
            set
            {
                var clampedValue = Mathf.Clamp(value, 0f, _maxWard);
                if (Mathf.Approximately(_currentWard.Value, clampedValue)) return;
                
                _currentWard.Value = clampedValue;
                _wardChangedSubject.OnNext(clampedValue);
            }
        }

        public Observable<float> OnWardChanged => _wardChangedSubject;

        /// <summary>
        /// Initializes the Ward Timer service.
        /// </summary>
        public WardTimerService(IPhaseStateMachine phaseStateMachine)
        {
            _phaseStateMachine = phaseStateMachine;
            _maxWard = BaseWardSec;
            _drainRate = DefaultDrainRate;
        }

        /// <summary>
        /// Initialize Ward with saved souls count and day penalties.
        /// </summary>
        public void Initialize(int ghostsSaved, float dayPenaltiesSec)
        {
            _maxWard = BaseWardSec + (ghostsSaved * WardPerGhostSec) - dayPenaltiesSec;
            _maxWard = Mathf.Clamp(_maxWard, BaseWardSec, BaseWardSec + (2 * WardPerGhostSec));
            CurrentWard = _maxWard;
        }

        /// <inheritdoc/>
        public void ApplyPenalty(float amount)
        {
            CurrentWard -= amount;
        }

        /// <summary>
        /// Get current Ward timer value.
        /// </summary>
        public float GetCurrentWard()
        {
            return CurrentWard;
        }

        /// <summary>
        /// Try to apply a cost to the Ward timer.
        /// </summary>
        public bool TryApplyCost(float cost)
        {
            // Only allow Ward costs during NightSurvival phase
            if (_phaseStateMachine.CurrentState != PhaseState.NightSurvival)
            {
                return false;
            }

            if (CurrentWard < cost)
            {
                return false;
            }

            CurrentWard -= cost;
            return true;
        }

        /// <summary>
        /// Update Ward timer with time-based drain.
        /// </summary>
        public void Update(float deltaTime)
        {
            if (_phaseStateMachine.CurrentState != PhaseState.NightSurvival)
            {
                return;
            }

            // Apply time-based drain
            if (_drainRate > 0 && CurrentWard > 0)
            {
                CurrentWard -= _drainRate * deltaTime;
            }
        }

        /// <summary>
        /// Set the drain rate using the Ngọc Cốt multiplicative formula.
        /// Formula: baseDrainRate × (1 + boneCount × 0.25) × (1 + hallucinationMultiplier)
        /// </summary>
        public void SetDrainRate(float baseDrainRate, int boneCount, float hallucinationMultiplier)
        {
            float boneMultiplier = 1f + (boneCount * 0.25f);
            float hallMultiplier = 1f + hallucinationMultiplier;
            _drainRate = baseDrainRate * boneMultiplier * hallMultiplier;
        }
    }
}
