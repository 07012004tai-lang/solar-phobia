using UnityEngine;
using R3;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Service handling Night Phase movement and skills.
    /// Implements TR-state-002: WASD, Sprint, Dash, Swing, Glide with Ward timer penalties.
    /// Uses CharacterController for movement, New Input System for actions.
    /// </summary>
    public interface INightPhaseMovementService
    {
        /// <summary>Process WASD movement input</summary>
        /// <returns>True if movement occurred</returns>
        bool TryMove(Vector3 inputDirection, PhaseState currentPhase);

        /// <summary>Activate Sprint (Shift) - costs 5s Ward</summary>
        /// <returns>True if sprint activated</returns>
        bool TrySprint(PhaseState currentPhase);

        /// <summary>Activate Spirit Dash - costs 5s Ward, quick forward burst</summary>
        /// <returns>True if dash activated</returns>
        bool TryDash(Vector3 direction, PhaseState currentPhase);

        /// <summary>Activate Swing (rope/grapple) - costs 2s Ward</summary>
        /// <returns>True if swing activated</returns>
        bool TrySwing(Vector3 targetPoint, PhaseState currentPhase);

        /// <summary>Activate Glide (hold jump) - costs 1s/s Ward</summary>
        /// <returns>True if glide activated</returns>
        bool TryGlide(bool isActive, PhaseState currentPhase);

        /// <summary>Get current Ward timer value</summary>
        float GetCurrentWard();

        /// <summary>Observable for Ward timer changes</summary>
        Observable<float> OnWardChanged { get; }
    }

    /// <summary>
    /// Implementation of Night Phase Movement Service.
    /// Enforces phase-gated actions per ADR-0001 phase contracts.
    /// </summary>
    public class NightPhaseMovementService : INightPhaseMovementService
    {
        // ── R3 Reactive State ──────────────────────────────────────
        private readonly Subject<float> _wardChangedSubject = new();

        // ── Dependencies ────────────────────────────────────────
        private readonly CharacterController _characterController;
        private readonly IWardTimerService _wardTimer;
        private readonly IAudioService _audioService;

        // ── State ────────────────────────────────────────────────
        private bool _isSprinting;
        private bool _isGliding;
        private float _currentGlideTime;

        // ── Constants ────────────────────────────────────────────
        private const float MoveSpeed = 5.0f;
        private const float SprintSpeedMultiplier = 1.5f;
        private const float SprintWardCost = 5.0f;
        private const float DashWardCost = 5.0f;
        private const float SwingWardCost = 2.0f;
        private const float GlideWardCostPerSecond = 1.0f;
        private const float DashDistance = 10.0f;
        private const float SwingDistance = 15.0f;

        public Observable<float> OnWardChanged => _wardChangedSubject;

        /// <summary>
        /// Initializes service with dependencies.
        /// </summary>
        public NightPhaseMovementService(CharacterController characterController,
                                         IWardTimerService wardTimer,
                                         IAudioService audioService)
        {
            _characterController = characterController ?? throw new System.ArgumentNullException(nameof(characterController));
            _wardTimer = wardTimer ?? throw new System.ArgumentNullException(nameof(wardTimer));
            _audioService = audioService ?? throw new System.ArgumentNullException(nameof(audioService));
        }

        /// <summary>
        /// Process WASD movement input. Only works in NightSurvival phase.
        /// </summary>
        public bool TryMove(Vector3 inputDirection, PhaseState currentPhase)
        {
            // Phase gating: Only NightSurvival allows movement
            if (currentPhase != PhaseState.NightSurvival)
            {
                Debug.LogWarning($"NightPhaseMovement: Move not allowed in {currentPhase}. Only NightSurvival permitted.");
                return false;
            }

            if (inputDirection == Vector3.zero)
                return false;

            // Calculate speed (apply sprint multiplier if active)
            float speed = MoveSpeed;
            if (_isSprinting)
            {
                speed *= SprintSpeedMultiplier;
                
                // Apply sprint Ward cost (per second)
                if (!_wardTimer.TryApplyCost(SprintWardCost * Time.deltaTime))
                {
                    _isSprinting = false; // Disable sprint if Ward too low
                    Debug.Log("NightPhaseMovement: Sprint disabled - insufficient Ward.");
                }
            }

            // Apply movement
            Vector3 move = inputDirection.normalized * speed * Time.deltaTime;
            _characterController.Move(move);

            Debug.Log($"NightPhaseMovement: Moved {move.magnitude:F2}m at speed {speed:F1}");
            return true;
        }

        /// <summary>
        /// Activate Sprint (Shift key). Costs 5s Ward.
        /// </summary>
        public bool TrySprint(PhaseState currentPhase)
        {
            // Phase gating: Only NightSurvival
            if (currentPhase != PhaseState.NightSurvival)
            {
                Debug.LogWarning($"NightPhaseMovement: Sprint not allowed in {currentPhase}.");
                return false;
            }

            // Check Ward timer
            if (!_wardTimer.TryApplyCost(SprintWardCost))
            {
                Debug.LogWarning("NightPhaseMovement: Cannot sprint - insufficient Ward (need 5s).");
                return false;
            }

            _isSprinting = true;
            _audioService?.PlaySprintSound();

            Debug.Log($"NightPhaseMovement: Sprint activated. Ward: -{SprintWardCost}s");
            return true;
        }

        /// <summary>
        /// Activate Spirit Dash. Costs 5s Ward, quick forward burst.
        /// </summary>
        public bool TryDash(Vector3 direction, PhaseState currentPhase)
        {
            // Phase gating: Only NightSurvival
            if (currentPhase != PhaseState.NightSurvival)
            {
                Debug.LogWarning($"NightPhaseMovement: Dash not allowed in {currentPhase}.");
                return false;
            }

            // Check Ward timer
            if (!_wardTimer.TryApplyCost(DashWardCost))
            {
                Debug.LogWarning("NightPhaseMovement: Cannot dash - insufficient Ward (need 5s).");
                return false;
            }

            // Execute dash (forward burst)
            Vector3 dashVector = direction.normalized * DashDistance;
            _characterController.Move(dashVector);

            _audioService?.PlayDashSound();

            Debug.Log($"NightPhaseMovement: Dash executed. Distance: {DashDistance}m, Ward: -{DashWardCost}s");
            return true;
        }

        /// <summary>
        /// Activate Swing (rope/grapple). Costs 2s Ward.
        /// </summary>
        public bool TrySwing(Vector3 targetPoint, PhaseState currentPhase)
        {
            // Phase gating: Only NightSurvival
            if (currentPhase != PhaseState.NightSurvival)
            {
                Debug.LogWarning($"NightPhaseMovement: Swing not allowed in {currentPhase}.");
                return false;
            }

            // Check Ward timer
            if (!_wardTimer.TryApplyCost(SwingWardCost))
            {
                Debug.LogWarning("NightPhaseMovement: Cannot swing - insufficient Ward (need 2s).");
                return false;
            }

            // Execute swing (move towards target)
            Vector3 toTarget = targetPoint - _characterController.transform.position;
            if (toTarget.magnitude > SwingDistance)
                toTarget = toTarget.normalized * SwingDistance;

            _characterController.Move(toTarget);

            _audioService?.PlaySwingSound();

            Debug.Log($"NightPhaseMovement: Swing to {targetPoint}. Distance: {toTarget.magnitude:F1}m, Ward: -{SwingWardCost}s");
            return true;
        }

        /// <summary>
        /// Activate Glide (hold jump). Costs 1s/s Ward.
        /// </summary>
        public bool TryGlide(bool isActive, PhaseState currentPhase)
        {
            // Phase gating: Only NightSurvival
            if (currentPhase != PhaseState.NightSurvival)
            {
                Debug.LogWarning($"NightPhaseMovement: Glide not allowed in {currentPhase}.");
                return false;
            }

            _isGliding = isActive;

            if (isActive)
            {
                // Apply glide Ward cost per second
                float costThisFrame = GlideWardCostPerSecond * Time.deltaTime;
                if (!_wardTimer.TryApplyCost(costThisFrame))
                {
                    _isGliding = false; // Disable glide if Ward too low
                    Debug.Log("NightPhaseMovement: Glide disabled - insufficient Ward.");
                    return false;
                }

                _currentGlideTime += Time.deltaTime;
                Debug.Log($"NightPhaseMovement: Gliding. Time: {_currentGlideTime:F1}s, Ward: -{costThisFrame:F1}s/s");
            }
            else
            {
                _currentGlideTime = 0f;
                Debug.Log("NightPhaseMovement: Glide ended.");
            }

            return true;
        }

        /// <summary>
        /// Get current Ward timer value.
        /// </summary>
        public float GetCurrentWard()
        {
            return _wardTimer.GetCurrentWard();
        }
    }
}
