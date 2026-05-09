// Assets/_Project/Application/Services/Interfaces/IMovementCalculator.cs
using UnityEngine;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Calculates the movement vector for the player each frame.
    /// Implements TR-player-002, TR-player-003: WASD base movement with configurable
    /// speed, gravity, and sprint multiplier.
    ///
    /// Separates movement math from Unity's CharacterController so the formula
    /// can be unit-tested in Editor without a scene or physics engine.
    /// </summary>
    public interface IMovementCalculator
    {
        /// <summary>
        /// Base movement speed in units/second.
        /// Default: 5.0, safe range: 2.0–8.0 (per GDD Tuning Knobs).
        /// </summary>
        float BaseMoveSpeed { get; set; }

        /// <summary>
        /// Sprint speed multiplier applied when <c>isSprinting</c> is true.
        /// Default: 1.8, safe range: 1.5–3.0 (per GDD Tuning Knobs).
        /// </summary>
        float SprintMultiplier { get; set; }

        /// <summary>
        /// Computes the world-space displacement vector for one frame.
        /// Returns <see cref="Vector3.zero"/> when <paramref name="mode"/> is not
        /// <see cref="PlayerInputMode.NightMovement"/>.
        /// </summary>
        /// <param name="horizontalInput">
        /// Normalised 2D input from WASD/stick (x = strafe, y = forward).
        /// </param>
        /// <param name="deltaTime">Frame delta time in seconds.</param>
        /// <param name="mode">Current player input mode — movement only active in NightMovement.</param>
        /// <param name="isSprinting">
        /// Whether sprint is currently active. When true, speed = baseMoveSpeed * sprintMultiplier.
        /// </param>
        /// <returns>
        /// Displacement vector to pass to <c>CharacterController.Move()</c>.
        /// Includes horizontal movement (with optional sprint) and downward gravity component.
        /// </returns>
        Vector3 CalculateMovement(Vector2 horizontalInput, float deltaTime, PlayerInputMode mode, bool isSprinting = false);
    }
}
