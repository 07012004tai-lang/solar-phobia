using System.Collections.Generic;
using UnityEngine;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Interface for cover detection service.
    /// </summary>
    public interface ICoverDetectionService
    {
        bool IsFullyInsideMound(Collider playerCollider, Collider moundCollider);
        bool IsPlayerExposed(Collider playerCollider, List<Collider> moundColliders);
        bool CheckMoundTypeCover(string moundType);
    }
}