using System;
using R3;
using UnityEngine;
using SolarPhobia.Application.Models;
using SolarPhobia.Application.Repositories;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Service handling Day Phase mechanics: Swap and Shove soul interactions.
    /// Implements TR-state-001 requirements for player-soul positioning.
    /// Uses R3 for reactive events and follows ADR-0001 architecture.
    /// </summary>
    public interface IDayPhaseMechanicsService
    {
        /// <summary>Attempt to swap player position with a soul at shadow edge</summary>
        /// <returns>True if swap initiated successfully</returns>
        bool TrySwap(string playerId, string soulId, PhaseState currentPhase);

        /// <summary>Force shove one soul into sunlight (abandonment)</summary>
        /// <returns>True if shove executed successfully</returns>
        bool TryShove(string soulId, PhaseState currentPhase);

        /// <summary>Observable event when a swap is initiated</summary>
        Observable<SwapEvent> OnSwapInitiated { get; }

        /// <summary>Observable event when a soul is shoved</summary>
        Observable<ShoveEvent> OnShoveExecuted { get; }

        /// <summary>Get the soul ID marked for sacrifice (if any)</summary>
        string GetSacrificedGhostId();

        /// <summary>Reset service state (called on PhaseState.Reset)</summary>
        void Reset();
    }

    /// <summary>Event emitted when player initiates a swap with a soul.</summary>
    public readonly struct SwapEvent
    {
        public readonly string PlayerId;
        public readonly string SoulId;
        public readonly float AnimationDuration; // 0.5s per GDD

        public SwapEvent(string playerId, string soulId)
        {
            PlayerId = playerId;
            SoulId = soulId;
            AnimationDuration = 0.5f;
        }
    }

    /// <summary>Event emitted when a soul is shoved into sunlight.</summary>
    public readonly struct ShoveEvent
    {
        public readonly string SoulId;
        public readonly string SacrificedGhostId; // Written to SoulRepository
        public readonly float AnimationDuration; // Screen shake + burn

        public ShoveEvent(string soulId, string sacrificiedGhostId)
        {
            SoulId = soulId;
            SacrificedGhostId = sacrificiedGhostId;
            AnimationDuration = 1.0f; // Screen shake + soul burn
        }
    }

    /// <summary>
    /// Implementation of Day Phase Mechanics Service.
    /// Enforces phase-gated swaps and shoves per ADR-0001 phase contracts.
    /// </summary>
    public class DayPhaseMechanicsService : IDayPhaseMechanicsService
    {
        // ── R3 Reactive Events ─────────────────────────────────────────
        private readonly Subject<SwapEvent> _swapSubject = new();
        private readonly Subject<ShoveEvent> _shoveSubject = new();

        // ── Dependencies ────────────────────────────────────────────────
        private readonly ISoulRepository _soulRepository;
        private readonly IAnimationService _animationService;
        private readonly IAudioService _audioService;

        // ── State ─────────────────────────────────────────────────────
        private string _sacrificedGhostId;
        private bool _isSwapInProgress;

        // ── Constants ────────────────────────────────────────────────
        private const float SwapAnimationDuration = 0.5f;
        private const float ShoveAnimationDuration = 1.0f;

        public Observable<SwapEvent> OnSwapInitiated => _swapSubject;
        public Observable<ShoveEvent> OnShoveExecuted => _shoveSubject;

        /// <summary>
        /// Initializes service with dependencies.
        /// </summary>
        public DayPhaseMechanicsService(ISoulRepository soulRepository, 
                                      IAnimationService animationService, 
                                      IAudioService audioService)
        {
            _soulRepository = soulRepository ?? throw new ArgumentNullException(nameof(soulRepository));
            _animationService = animationService ?? throw new ArgumentNullException(nameof(animationService));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }

        /// <summary>
        /// Attempts to swap player position with a soul at shadow edge.
        /// Only allowed during DayService phase (per ADR-0001 phase contracts).
        /// </summary>
        public bool TrySwap(string playerId, string soulId, PhaseState currentPhase)
        {
            // Phase gating: Swap only allowed in DayService
            if (currentPhase != PhaseState.DayService)
            {
                Debug.LogWarning($"DayPhaseMechanics: Swap not allowed in {currentPhase}. Only DayService permitted.");
                return false;
            }

            // Validate inputs
            if (string.IsNullOrEmpty(soulId) || string.IsNullOrEmpty(playerId))
            {
                Debug.LogError("DayPhaseMechanics: Player ID and Soul ID must be non-empty.");
                return false;
            }

            if (!_soulRepository.IsAtShadowEdge(soulId))
            {
                Debug.LogWarning($"DayPhaseMechanics: Soul {soulId} is not at shadow edge.");
                return false;
            }

            // Check if swap already in progress (prevent spam)
            if (_isSwapInProgress)
            {
                Debug.LogWarning("DayPhaseMechanics: Swap already in progress.");
                return false;
            }

            // Execute swap
            _isSwapInProgress = true;
            _soulRepository.SwapPositions(playerId, soulId);

            // Play animation and sound (0.5s animation per GDD)
            _animationService?.PlaySwapAnimation(playerId, soulId, SwapAnimationDuration);
            _audioService?.PlaySwapSound();

            // Emit event for animation system
            var swapEvent = new SwapEvent(playerId, soulId);
            _swapSubject.OnNext(swapEvent);

            Debug.Log($"DayPhaseMechanics: Swap initiated between {playerId} and {soulId}. Animation: {SwapAnimationDuration}s");

            // Reset swap flag (in real implementation, this would be after animation completes)
            SimulateAnimationCompletion();

            return true;
        }

        /// <summary>
        /// Forces a soul to be shoved into sunlight (abandonment).
        /// Called at Collapse phase end or when timer expires.
        /// Writes sacrificied_ghost_id to SoulRepository before ChoiceLock transition.
        /// </summary>
        public bool TryShove(string soulId, PhaseState currentPhase)
        {
            // Phase gating: Shove allowed in DayService (Collapse) and ChoiceLock
            if (currentPhase != PhaseState.DayService && currentPhase != PhaseState.ChoiceLock)
            {
                Debug.LogWarning($"DayPhaseMechanics: Shove not allowed in {currentPhase}. DayService/ChoiceLock required.");
                return false;
            }

            if (string.IsNullOrEmpty(soulId))
            {
                Debug.LogError("DayPhaseMechanics: Soul ID must be non-empty for shove.");
                return false;
            }

            var soul = _soulRepository.GetSoul(soulId);
            if (soul == null)
            {
                Debug.LogError($"DayPhaseMechanics: Soul {soulId} not found.");
                return false;
            }

            // Mark soul as abandoned (writes to SoulRepository)
            _soulRepository.MarkAbandoned(soulId);

            // Write sacrificied_ghost_id (persisted to night phase per GDD)
            _sacrificedGhostId = soulId;
            _soulRepository.SetSacrificedGhostId(soulId);

            // Trigger visual/audio feedback
            _animationService?.PlayShoveAnimation(null, soulId);
            _audioService?.PlayShoveImpact();
            _audioService?.PlaySoulBurn();

            // Emit shove event (screen shake + soul burn)
            var shoveEvent = new ShoveEvent(soulId, _sacrificedGhostId);
            _shoveSubject.OnNext(shoveEvent);

            Debug.Log($"DayPhaseMechanics: Soul {soulId} shoved into sunlight. SacrificedGhostId set: {_sacrificedGhostId}. Animation: {ShoveAnimationDuration}s");

            return true;
        }

        /// <summary>
        /// Returns the soul ID marked for sacrifice (sacrificied_ghost_id).
        /// This value persists to NightSurvival phase for karma hazards.
        /// </summary>
        public string GetSacrificedGhostId()
        {
            return _sacrificedGhostId;
        }

        /// <summary>
        /// Resets service state (called on PhaseState.Reset).
        /// Clears sacrificied_ghost_id and swap state.
        /// </summary>
        public void Reset()
        {
            _sacrificedGhostId = null;
            _isSwapInProgress = false;
            Debug.Log("DayPhaseMechanics: Service reset.");
        }

        // ── Private Methods ─────────────────────────────────────────

        /// <summary>
        /// Simulates animation completion (0.5s swap animation).
        /// In real implementation, this would be driven by Unity's Update loop or DOTween.
        /// </summary>
        private void SimulateAnimationCompletion()
        {
            // Placeholder: In actual game, animation system would call this after 0.5s
            _isSwapInProgress = false;
        }
    }
}
