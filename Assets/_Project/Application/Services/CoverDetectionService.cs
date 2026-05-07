/* Assets/_Project/Application/Services/CoverDetectionService.cs */

using UnityEngine;
using System.Collections.Generic;
using R3;
using VContainer;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Interface for cover detection service.
    /// </summary>
    public interface ICoverDetectionService
    {
        /// <summary>
        /// Checks if the player collider is fully inside the mound collider.
        /// </summary>
        /// <param name="playerCollider">The player's collider.</param>
        /// <param name="moundCollider">The mound's collider.</param>
        /// <returns>True if player is fully inside the mound, else false.</returns>
        bool IsFullyInsideMound(Collider playerCollider, Collider moundCollider);

        /// <summary>
        /// Checks if the player is exposed (not fully inside any mound).
        /// </summary>
        /// <param name="playerCollider">The player's collider.</param>
        /// <param name="moundColliders">List of all mound colliders in the scene.</param>
        /// <returns>True if player is exposed, else false.</returns>
        bool IsPlayerExposed(Collider playerCollider, List<Collider> moundColliders);

        /// <summary>
        /// Checks if a mound type provides valid cover.
        /// </summary>
        /// <param name="moundType">The type of the mound (e.g., SafeMound).</param>
        /// <returns>True if the mound type provides cover, else false.</returns>
        bool CheckMoundTypeCover(string moundType);
    }

    /// <summary>
    /// Service for detecting player cover status against mounds.
    /// </summary>
    public class CoverDetectionService : ICoverDetectionService
    {
        // ── Dependencies ──────────────────────────────
        private readonly ReadOnlyReactiveProperty<PhaseState> _currentPhase;

        /// <summary>
        /// Initializes a new instance of the CoverDetectionService class.
        /// </summary>
        /// <param name="currentPhase">The current phase reactive property from PhaseStateMachine.</param>
        [Inject]
        public CoverDetectionService(ReadOnlyReactiveProperty<PhaseState> currentPhase)
        {
            _currentPhase = currentPhase;
        }

        /// <inheritdoc />
        public bool IsFullyInsideMound(Collider playerCollider, Collider moundCollider)
        {
            if (!IsNightSurvivalPhase()) return false;
            if (playerCollider == null || moundCollider == null) return false;

            Bounds playerBounds = playerCollider.bounds;
            Bounds moundBounds = moundCollider.bounds;

            // Check if player AABB is fully contained within mound AABB
            return moundBounds.Contains(playerBounds.min) && moundBounds.Contains(playerBounds.max);
        }

        /// <inheritdoc />
        public bool IsPlayerExposed(Collider playerCollider, List<Collider> moundColliders)
        {
            if (!IsNightSurvivalPhase()) return false;
            if (playerCollider == null || moundColliders == null) return false;

            foreach (Collider mound in moundColliders)
            {
                if (IsFullyInsideMound(playerCollider, mound))
                {
                    return false; // Fully inside a mound, not exposed
                }
            }
            return true; // Not fully inside any mound, exposed
        }

        /// <inheritdoc />
        public bool CheckMoundTypeCover(string moundType)
        {
            if (!IsNightSurvivalPhase()) return false;
            if (string.IsNullOrEmpty(moundType)) return false;

            // Supported mound types per Story 004
            return moundType == "SafeMound" || moundType == "CursedMound" || moundType == "FalseSafeMound";
        }

        // ── Phase Helpers ──────────────────────────────
        private bool IsNightSurvivalPhase()
        {
            return _currentPhase != null && _currentPhase.Value == PhaseState.NightSurvival;
        }
    }
}