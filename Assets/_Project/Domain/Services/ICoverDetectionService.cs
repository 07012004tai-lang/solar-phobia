// Assets/_Project/Domain/Services/ICoverDetectionService.cs
using UnityEngine;
using System.Collections.Generic;

namespace SolarPhobia.Domain
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
}
