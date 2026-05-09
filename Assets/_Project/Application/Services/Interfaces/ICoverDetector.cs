// Assets/_Project/Application/Services/Interfaces/ICoverDetector.cs
using R3;
using UnityEngine;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Evaluates whether the player is fully inside a cover volume.
    /// Implements TR-player-004: Cover Detection — Full Containment Validation.
    ///
    /// Cover registers only when the player bounds are fully contained within
    /// a cover volume bounds (partial overlap = exposed).
    /// The containment fraction threshold is configurable via <see cref="CoverEnterThreshold"/>.
    ///
    /// Exposes <see cref="IsInCover"/> as a reactive property — fires only on state change.
    /// Cover check is only active when <see cref="PlayerInputMode"/> is
    /// <see cref="PlayerInputMode.NightMovement"/>.
    /// </summary>
    public interface ICoverDetector
    {
        /// <summary>
        /// Whether the player is currently fully inside a cover volume.
        /// Reactive — fires only when the value changes.
        /// </summary>
        ReadOnlyReactiveProperty<bool> IsInCover { get; }

        /// <summary>
        /// Fraction of the player bounds that must be inside the cover volume
        /// for cover to register. Default: 1.0 (fully inside), range: 0.8–1.0.
        /// </summary>
        float CoverEnterThreshold { get; set; }

        /// <summary>
        /// Evaluates cover state for the current frame.
        /// Call once per frame from the player controller's Update loop.
        /// Does nothing when <paramref name="mode"/> is not
        /// <see cref="PlayerInputMode.NightMovement"/>.
        /// </summary>
        /// <param name="playerBounds">World-space bounds of the player collider.</param>
        /// <param name="coverVolumes">All active cover volume bounds in the scene.</param>
        /// <param name="mode">Current player input mode.</param>
        void CheckCover(Bounds playerBounds, Bounds[] coverVolumes, PlayerInputMode mode);
    }
}
