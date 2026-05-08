/* Assets/_Project/Application/Services/CoverDetectionService.cs */

using UnityEngine;
using System.Collections.Generic;
using System;
using R3;
using VContainer;
using SolarPhobia.Domain.ValueObjects;

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
        bool IsFullyInsideMound(Collider playerCollider, Collider moundCollider);

        /// <summary>
        /// Checks if the player is exposed (not fully inside any mound).
        /// </summary>
        bool IsPlayerExposed(Collider playerCollider, List<Collider> moundColliders);

        /// <summary>
        /// Checks if a mound type provides valid cover.
        /// </summary>
        bool CheckMoundTypeCover(string moundType);
    }

    /// <summary>
    /// Service for detecting player cover status against mounds.
    /// </summary>
    public class CoverDetectionService : ICoverDetectionService, IDisposable
    {
        // ── Dependencies ──────────────────────────────
        private PhaseState _currentPhaseValue;
        private IDisposable _phaseSubscription;

        // ── Phase Helpers ──────────────────────────────
        private bool IsNightSurvivalPhase()
        {
            return _currentPhaseValue == PhaseState.NightSurvival;
        }

        /// <summary>
        /// Initializes a new instance of the CoverDetectionService class.
        /// </summary>
        [Inject]
        public CoverDetectionService(ReadOnlyReactiveProperty<PhaseState> currentPhase)
        {
            _phaseSubscription = currentPhase
                .AsObservable()
                .Subscribe(newPhase => _currentPhaseValue = newPhase);
        }

        /// <inheritdoc />
        public bool IsFullyInsideMound(Collider playerCollider, Collider moundCollider)
        {
            if (!IsNightSurvivalPhase()) return false;
            if (playerCollider == null || moundCollider == null) return false;

            Bounds playerBounds = playerCollider.bounds;
            Bounds moundBounds = moundCollider.bounds;

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

            return moundType == "SafeMound" || moundType == "CursedMound" || moundType == "FalseSafeMound";
        }

        /// <summary>
        /// Dispose all subscriptions.
        /// </summary>
        public void Dispose()
        {
            _phaseSubscription?.Dispose();
        }
    }
}