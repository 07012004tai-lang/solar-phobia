// Assets/_Project/Application/Services/BossSearchlightService.cs
using System;
using R3;
using UnityEngine;
using VContainer;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Interface for boss searchlight sweep and strike logic.
    /// </summary>
    public interface IBossSearchlightService
    {
        /// <summary>Activate the boss searchlight sweep.</summary>
        void ActivateSearchlight();

        /// <summary>Deactivate the boss searchlight.</summary>
        void DeactivateSearchlight();

        /// <summary>Check if the player is exposed to the searchlight.</summary>
        /// <param name="playerPosition">Current player world position.</param>
        /// <param name="isInCover">Whether the player is in valid cover.</param>
        /// <returns>True if player is exposed to the active searchlight.</returns>
        bool IsPlayerExposed(Vector3 playerPosition, bool isInCover);

        /// <summary>Execute a strike against the exposed player.</summary>
        void ExecuteStrike();

        /// <summary>Observable that triggers when telegraph state changes.</summary>
        Observable<bool> OnTelegraphActive { get; }

        /// <summary>Observable that pushes ward penalty amounts when strike hits.</summary>
        Observable<float> OnWardPenalty { get; }
    }

    /// <summary>
    /// Implements boss searchlight sweep, telegraph, and strike logic gated to NightSurvival phase.
    /// </summary>
    public class BossSearchlightService : IBossSearchlightService, IDisposable
    {
        // ── Dependencies ──────────────────────────────
        private readonly IPhaseStateMachine _phaseStateMachine;
        private ICoverDetectionService _coverDetectionService;

        // ── State ──────────────────────────────
        private PhaseState _currentPhaseValue;
        private readonly ReactiveProperty<bool> _isActive = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<bool> _telegraphActive = new ReactiveProperty<bool>(false);
        private readonly ReactiveProperty<float> _wardPenalty = new ReactiveProperty<float>(0f);
        private IDisposable _phaseSubscription;
        private IDisposable _updateSubscription;
        private IDisposable _telegraphTimer;

        // ── Sweep Parameters ──────────────────────────────
        private float _sweepSpeed = 30f;
        private float _currentSweepAngle = -45f;
        private const float MaxSweepAngle = 45f;
        private const float MinSweepAngle = -45f;
        private const float ConeAngle = 15f;
        private Vector3 _searchlightOrigin = new Vector3(0f, 15f, -20f);
        private bool _isSweepingRight = true;

        // ── Telegraph Parameters ──────────────────────────────
        private const float TelegraphDuration = 2f;
        private const float StrikePenalty = 30f;

        /// <summary>Observable that triggers when telegraph state changes.</summary>
        public Observable<bool> OnTelegraphActive => _telegraphActive;

        /// <summary>Observable that pushes ward penalty amounts when strike hits.</summary>
        public Observable<float> OnWardPenalty => _wardPenalty;

        /// <summary>
        /// Constructor with dependency injection via VContainer.
        /// </summary>
        [Inject]
        public BossSearchlightService(
            IPhaseStateMachine phaseStateMachine,
            ICoverDetectionService coverDetectionService)
        {
            _phaseStateMachine = phaseStateMachine;
            _coverDetectionService = coverDetectionService;

            _phaseSubscription = _phaseStateMachine.CurrentPhase
                .AsObservable()
                .Subscribe(newPhase => {
                    _currentPhaseValue = newPhase;
                    OnPhaseChanged(newPhase);
                });

            _updateSubscription = Observable.EveryUpdate()
                .Where(_ => _isActive.Value)
                .Subscribe(_ => UpdateSweep());
        }

        // ── Public Methods ──────────────────────────────
        /// <inheritdoc/>
        public void ActivateSearchlight()
        {
            if (_currentPhaseValue != PhaseState.NightSurvival)
                return;

            _isActive.Value = true;
            _currentSweepAngle = MinSweepAngle;
            _isSweepingRight = true;
        }

        /// <inheritdoc/>
        public void DeactivateSearchlight()
        {
            _isActive.Value = false;
            _telegraphActive.Value = false;
            _telegraphTimer?.Dispose();
            _currentSweepAngle = MinSweepAngle;
        }

        /// <inheritdoc/>
        public bool IsPlayerExposed(Vector3 playerPosition, bool isInCover)
        {
            if (!_isActive.Value || isInCover)
            {
                ClearTelegraph();
                return false;
            }

            bool isInCone = CheckConeIntersection(playerPosition);
            if (isInCone && !_telegraphActive.Value)
            {
                StartTelegraph();
            }
            else if (!isInCone)
            {
                ClearTelegraph();
            }

            return isInCone;
        }

        /// <inheritdoc/>
        public void ExecuteStrike()
        {
            if (!_isActive.Value)
                return;

            _wardPenalty.Value = StrikePenalty;
            Debug.Log($"Boss searchlight strike hit! Ward penalty: {StrikePenalty}s");

            ClearTelegraph();
        }

        // ── Private Methods ──────────────────────────────
        private void UpdateSweep()
        {
            float delta = Time.deltaTime * _sweepSpeed * (_isSweepingRight ? 1f : -1f);
            _currentSweepAngle += delta;

            if (_isSweepingRight && _currentSweepAngle >= MaxSweepAngle)
            {
                _currentSweepAngle = MaxSweepAngle;
                _isSweepingRight = false;
            }
            else if (!_isSweepingRight && _currentSweepAngle <= MinSweepAngle)
            {
                _currentSweepAngle = MinSweepAngle;
                _isSweepingRight = true;
            }
        }

        private bool CheckConeIntersection(Vector3 playerPosition)
        {
            Vector3 toPlayer = playerPosition - _searchlightOrigin;
            toPlayer.y = 0f;
            if (toPlayer.sqrMagnitude < 0.001f)
                return false;

            Quaternion sweepRotation = Quaternion.Euler(0f, _currentSweepAngle, 0f);
            Vector3 sweepDirection = sweepRotation * Vector3.forward;
            float angle = Vector3.Angle(sweepDirection, toPlayer.normalized);
            return angle <= ConeAngle;
        }

        private void StartTelegraph()
        {
            _telegraphActive.Value = true;
            _telegraphTimer = Observable.Timer(TimeSpan.FromSeconds(TelegraphDuration))
                .Subscribe(_ => ExecuteStrike());
        }

        private void ClearTelegraph()
        {
            if (_telegraphActive.Value)
            {
                _telegraphActive.Value = false;
                _telegraphTimer?.Dispose();
            }
        }

        private void OnPhaseChanged(PhaseState newPhase)
        {
            if (newPhase != PhaseState.NightSurvival && _isActive.Value)
            {
                DeactivateSearchlight();
            }
        }

        /// <summary>Dispose all subscriptions and reactive properties.</summary>
        public void Dispose()
        {
            _phaseSubscription?.Dispose();
            _updateSubscription?.Dispose();
            _telegraphTimer?.Dispose();
            _isActive?.Dispose();
            _telegraphActive?.Dispose();
            _wardPenalty?.Dispose();
        }
    }
}