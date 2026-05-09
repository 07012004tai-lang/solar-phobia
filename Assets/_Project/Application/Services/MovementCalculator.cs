// Assets/_Project/Application/Services/MovementCalculator.cs
using UnityEngine;
using SolarPhobia.Domain.ValueObjects;
using VContainer;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Calculates the player movement displacement vector each frame.
    /// Implements TR-player-002: WASD base movement with configurable speed and gravity.
    ///
    /// Formula:
    ///   horizontal = new Vector3(input.x, 0, input.y) * baseMoveSpeed * deltaTime
    ///   gravity    = Vector3.down * GravityAcceleration * deltaTime
    ///   result     = horizontal + gravity   (when mode == NightMovement)
    ///   result     = Vector3.zero           (when mode != NightMovement)
    ///
    /// This class is a pure calculation service — it holds no Unity scene state
    /// and can be fully tested in the Editor without a CharacterController.
    /// </summary>
    public class MovementCalculator : IMovementCalculator
    {
        // ── Constants ─────────────────────────────────────────────
        /// <summary>Gravity acceleration in units/s². Matches Unity's default (9.81 m/s²).</summary>
        public const float GravityAcceleration = 9.81f;

        /// <summary>Default base movement speed (units/second).</summary>
        public const float DefaultBaseMoveSpeed = 5.0f;

        /// <summary>Minimum allowed base movement speed (per GDD Tuning Knobs).</summary>
        public const float MinBaseMoveSpeed = 2.0f;

        /// <summary>Maximum allowed base movement speed (per GDD Tuning Knobs).</summary>
        public const float MaxBaseMoveSpeed = 8.0f;

        /// <summary>Default sprint speed multiplier (per GDD Tuning Knobs).</summary>
        public const float DefaultSprintMultiplier = 1.8f;

        /// <summary>Minimum allowed sprint multiplier (per GDD Tuning Knobs).</summary>
        public const float MinSprintMultiplier = 1.5f;

        /// <summary>Maximum allowed sprint multiplier (per GDD Tuning Knobs).</summary>
        public const float MaxSprintMultiplier = 3.0f;

        // ── State ─────────────────────────────────────────────────
        private float _baseMoveSpeed = DefaultBaseMoveSpeed;
        private float _sprintMultiplier = DefaultSprintMultiplier;

        // ── Public Interface ───────────────────────────────────────
        /// <inheritdoc/>
        public float BaseMoveSpeed
        {
            get => _baseMoveSpeed;
            set => _baseMoveSpeed = Mathf.Clamp(value, MinBaseMoveSpeed, MaxBaseMoveSpeed);
        }

        /// <inheritdoc/>
        public float SprintMultiplier
        {
            get => _sprintMultiplier;
            set => _sprintMultiplier = Mathf.Clamp(value, MinSprintMultiplier, MaxSprintMultiplier);
        }

        // ── Constructor ────────────────────────────────────────────
        [Inject]
        public MovementCalculator() { }

        // ── IMovementCalculator ────────────────────────────────────
        /// <inheritdoc/>
        public Vector3 CalculateMovement(Vector2 horizontalInput, float deltaTime, PlayerInputMode mode, bool isSprinting = false)
        {
            if (mode != PlayerInputMode.NightMovement)
            {
                return Vector3.zero;
            }

            float speed = _baseMoveSpeed * (isSprinting ? _sprintMultiplier : 1f);

            // Horizontal displacement: WASD input mapped to XZ plane
            var horizontal = new Vector3(horizontalInput.x, 0f, horizontalInput.y)
                             * speed
                             * deltaTime;

            // Gravity: constant downward acceleration each frame
            var gravity = Vector3.down * GravityAcceleration * deltaTime;

            return horizontal + gravity;
        }
    }
}
