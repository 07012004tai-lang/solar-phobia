// Assets/_Project/Application/Services/CoverDetector.cs
using R3;
using UnityEngine;
using SolarPhobia.Domain.ValueObjects;
using VContainer;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Evaluates whether the player is fully inside a cover volume each frame.
    /// Implements TR-player-004: Cover Detection — Full Containment Validation.
    ///
    /// Algorithm:
    ///   For each cover volume, check whether the player bounds are contained
    ///   within the volume bounds by the configured threshold fraction.
    ///   At threshold = 1.0: player min >= volume min AND player max <= volume max (all axes).
    ///   At threshold < 1.0: allows a small margin — the player's "effective bounds"
    ///   (shrunk by 1-threshold on each side) must fit inside the volume.
    ///
    /// IsInCover fires only on state transitions — not every frame.
    /// Cover check is skipped when mode != NightMovement.
    /// </summary>
    public class CoverDetector : ICoverDetector
    {
        // ── Constants ─────────────────────────────────────────────
        /// <summary>Default cover enter threshold (fully inside).</summary>
        public const float DefaultThreshold = 1.0f;

        /// <summary>Minimum allowed threshold (per GDD Tuning Knobs).</summary>
        public const float MinThreshold = 0.8f;

        /// <summary>Maximum allowed threshold.</summary>
        public const float MaxThreshold = 1.0f;

        // ── R3 Reactive State ──────────────────────────────────────
        private readonly ReactiveProperty<bool> _isInCover = new(false);

        // ── State ─────────────────────────────────────────────────
        private float _threshold = DefaultThreshold;

        // ── Public Interface ───────────────────────────────────────
        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<bool> IsInCover => _isInCover;

        /// <inheritdoc/>
        public float CoverEnterThreshold
        {
            get => _threshold;
            set => _threshold = Mathf.Clamp(value, MinThreshold, MaxThreshold);
        }

        // ── Constructor ────────────────────────────────────────────
        [Inject]
        public CoverDetector() { }

        // ── ICoverDetector ─────────────────────────────────────────
        /// <inheritdoc/>
        public void CheckCover(Bounds playerBounds, Bounds[] coverVolumes, PlayerInputMode mode)
        {
            if (mode != PlayerInputMode.NightMovement)
            {
                // Outside NightSurvival — do not update cover state
                return;
            }

            bool covered = false;

            if (coverVolumes != null)
            {
                foreach (var volume in coverVolumes)
                {
                    if (IsContainedWithinThreshold(playerBounds, volume, _threshold))
                    {
                        covered = true;
                        break;
                    }
                }
            }

            // Only update ReactiveProperty when value changes (avoids redundant emissions)
            if (_isInCover.Value != covered)
            {
                _isInCover.Value = covered;
            }
        }

        // ── Private Methods ────────────────────────────────────────
        /// <summary>
        /// Returns true when <paramref name="player"/> is contained within
        /// <paramref name="volume"/> by at least <paramref name="threshold"/> fraction.
        /// Public for unit testing.
        /// </summary>
        public static bool IsContainedWithinThreshold(Bounds player, Bounds volume, float threshold)
        {
            if (threshold >= 1.0f)
            {
                // Strict full containment — all axes
                return player.min.x >= volume.min.x
                    && player.min.y >= volume.min.y
                    && player.min.z >= volume.min.z
                    && player.max.x <= volume.max.x
                    && player.max.y <= volume.max.y
                    && player.max.z <= volume.max.z;
            }

            // Partial threshold: shrink player bounds by (1 - threshold) on each side
            // and check if the shrunk bounds fit inside the volume
            var shrink = player.extents * (1f - threshold);
            var shrunkMin = player.min + shrink;
            var shrunkMax = player.max - shrink;

            return shrunkMin.x >= volume.min.x
                && shrunkMin.y >= volume.min.y
                && shrunkMin.z >= volume.min.z
                && shrunkMax.x <= volume.max.x
                && shrunkMax.y <= volume.max.y
                && shrunkMax.z <= volume.max.z;
        }
    }
}
