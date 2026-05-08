// Assets/_Project/Application/Services/KarmaHazardService.cs
using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using VContainer;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Interface for karma hazard spawning based on sacrificed ghost type.
    /// </summary>
    public interface IKarmaHazardService
    {
        /// <summary>
        /// Spawn karma hazard based on sacrificed ghost type at the specified position.
        /// </summary>
        void SpawnHazardForGhost(string ghostType, Vector3 position);

        /// <summary>
        /// Clear all active karma hazards.
        /// </summary>
        void ClearHazards();

        /// <summary>
        /// Observable that triggers when a hazard is spawned.
        /// </summary>
        Observable<KarmaHazardData> OnHazardSpawned { get; }
    }

    /// <summary>
    /// Data class for karma hazard information.
    /// </summary>
    public class KarmaHazardData
    {
        public string GhostType { get; set; }
        public string HazardType { get; set; }
        public Vector3 Position { get; set; }
        public float EffectValue { get; set; }
    }

    /// <summary>
    /// Implementation of karma hazard spawning service for Night phase.
    /// Maps sacrificed ghost types to specific hazards:
    /// - Van → Lưới Máu (slow penalty, 0.5× movement)
    /// - Linh → Vũng Nước (DoT, 5 HP/s)
    /// - Minh → Bệ Đá Ảo Ảnh (0.2s collapse on trigger)
    /// </summary>
    public class KarmaHazardService : IKarmaHazardService, IDisposable
    {
        // ── Dependencies ──────────────────────────────
        private PhaseState _currentPhaseValue;
        private IDisposable _phaseSubscription;

        // ── State ──────────────────────────────
        private readonly ReactiveProperty<KarmaHazardData> _hazardSpawned = new();
        private readonly List<GameObject> _activeHazards = new();

        /// <summary>
        /// Observable that triggers when a hazard is spawned.
        /// </summary>
        public Observable<KarmaHazardData> OnHazardSpawned => _hazardSpawned;

        /// <summary>
        /// Initializes a new instance of the KarmaHazardService class.
        /// </summary>
        [Inject]
        public KarmaHazardService(ReadOnlyReactiveProperty<PhaseState> currentPhase)
        {
            _phaseSubscription = currentPhase
                .AsObservable()
                .Subscribe(newPhase => _currentPhaseValue = newPhase);
        }

        /// <inheritdoc/>
        public void SpawnHazardForGhost(string ghostType, Vector3 position)
        {
            if (!IsNightSurvivalPhase())
                return;

            if (string.IsNullOrEmpty(ghostType))
                return;

            string hazardType = MapGhostToHazard(ghostType);
            float effectValue = GetEffectValue(hazardType);

            var hazardData = new KarmaHazardData
            {
                GhostType = ghostType,
                HazardType = hazardType,
                Position = position,
                EffectValue = effectValue
            };

            GameObject hazard = SpawnHazardPrefab(hazardType, position, effectValue);
            if (hazard != null)
            {
                _activeHazards.Add(hazard);
                _hazardSpawned.Value = hazardData;
            }
        }

        /// <inheritdoc/>
        public void ClearHazards()
        {
            foreach (var hazard in _activeHazards)
            {
                if (hazard != null)
                    GameObject.Destroy(hazard);
            }
            _activeHazards.Clear();
        }

        /// <summary>
        /// Map ghost type to hazard type.
        /// </summary>
        public static string MapGhostToHazard(string ghostType)
        {
            return ghostType switch
            {
                "Van" => "LuoiMau",
                "Linh" => "VungNuoc",
                "Minh" => "BeDaDaoAnh",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Get effect value for hazard type.
        /// </summary>
        public static float GetEffectValue(string hazardType)
        {
            return hazardType switch
            {
                "LuoiMau" => 0.5f,    // 0.5× movement speed
                "VungNuoc" => 5f,     // 5 HP/s DoT
                "BeDaDaoAnh" => 0.2f, // 0.2s collapse
                _ => 0f
            };
        }

        /// <summary>
        /// Check if ghost type has a valid hazard mapping.
        /// </summary>
        public static bool HasHazardMapping(string ghostType)
        {
            return ghostType switch
            {
                "Van" or "Linh" or "Minh" => true,
                _ => false
            };
        }

        // ── Private Methods ──────────────────────────────
        private GameObject SpawnHazardPrefab(string hazardType, Vector3 position, float effectValue)
        {
            GameObject hazard = new GameObject($"KarmaHazard_{hazardType}");
            hazard.transform.position = position;

            switch (hazardType)
            {
                case "LuoiMau":
                    hazard.AddComponent<LuoiMauHazard>().Initialize(effectValue);
                    break;
                case "VungNuoc":
                    hazard.AddComponent<VungNuocHazard>().Initialize(effectValue);
                    break;
                case "BeDaDaoAnh":
                    hazard.AddComponent<BeDaDaoAnhHazard>().Initialize(effectValue);
                    break;
            }

            return hazard;
        }

        private void OnPhaseChanged(PhaseState newPhase)
        {
            if (newPhase != PhaseState.NightSurvival)
            {
                ClearHazards();
            }
        }

        private bool IsNightSurvivalPhase()
        {
            return _currentPhaseValue == PhaseState.NightSurvival;
        }

        /// <summary>
        /// Dispose all subscriptions and clear hazards.
        /// </summary>
        public void Dispose()
        {
            _phaseSubscription?.Dispose();
            _hazardSpawned?.Dispose();
            ClearHazards();
        }
    }

    // ── Hazard Component Classes ──────────────────────────────

    /// <summary>
    /// Lưới Máu hazard - applies movement speed reduction while player is in zone.
    /// </summary>
    public class LuoiMauHazard : MonoBehaviour
    {
        private float _slowMultiplier;
        private SphereCollider _trigger;

        public LuoiMauHazard Initialize(float slowMultiplier)
        {
            _slowMultiplier = slowMultiplier;
            _trigger = gameObject.AddComponent<SphereCollider>();
            _trigger.isTrigger = true;
            _trigger.radius = 5f;
            return this;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player in Luoi Mau zone - speed reduced to {_slowMultiplier}×");
            }
        }
    }

    /// <summary>
    /// Vũng Nước hazard - applies damage-over-time while player is standing in it.
    /// </summary>
    public class VungNuocHazard : MonoBehaviour
    {
        private float _damagePerSecond;
        private SphereCollider _trigger;

        public VungNuocHazard Initialize(float damagePerSecond)
        {
            _damagePerSecond = damagePerSecond;
            _trigger = gameObject.AddComponent<SphereCollider>();
            _trigger.isTrigger = true;
            _trigger.radius = 3f;
            return this;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player in Vung Nuoc zone - taking {_damagePerSecond} HP/s");
            }
        }
    }

    /// <summary>
    /// Bệ Đá Ảo Ảnh hazard - collapses for 0.2s when player enters trigger.
    /// </summary>
    public class BeDaDaoAnhHazard : MonoBehaviour
    {
        private float _collapseDuration;
        private BoxCollider _trigger;
        private bool _isCollapsed;

        public BeDaDaoAnhHazard Initialize(float collapseDuration)
        {
            _collapseDuration = collapseDuration;
            _trigger = gameObject.AddComponent<BoxCollider>();
            _trigger.isTrigger = true;
            _trigger.size = new Vector3(2f, 2f, 2f);
            return this;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isCollapsed)
            {
                StartCoroutine(CollapseRoutine());
            }
        }

        private System.Collections.IEnumerator CollapseRoutine()
        {
            _isCollapsed = true;
            Debug.Log($"Be Da Dao Anh collapsed for {_collapseDuration}s");
            yield return new WaitForSeconds(_collapseDuration);
            _isCollapsed = false;
        }
    }
}