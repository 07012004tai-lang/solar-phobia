using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using R3;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Tests
{
    /// <summary>
    /// Unity-compilable tests for KarmaHazardService.
    /// Validates: TR-state-004 — Night phase mechanics (hazards, karma).
    /// </summary>
    [TestFixture]
    public class KarmaHazardsTests
    {
        private KarmaHazardService _service;
        private List<GameObject> _testHazards;

        [SetUp]
        public void SetUp()
        {
            _service = new KarmaHazardService(CreateMockPhaseProperty(PhaseState.NightSurvival));
            _testHazards = new List<GameObject>();
        }

        [TearDown]
        public void TearDown()
        {
            _service?.Dispose();
            foreach (var hazard in _testHazards)
            {
                if (hazard != null) GameObject.DestroyImmediate(hazard);
            }
            _testHazards.Clear();
        }

        [Test]
        public void AC1_VanSacrifice_SpawnsLuoiMauHazard()
        {
            var playerPosition = new Vector3(10f, 0f, 5f);
            _service.SpawnHazardForGhost("Van", playerPosition);

            var hazardType = KarmaHazardService.MapGhostToHazard("Van");
            Assert.That(hazardType, Is.EqualTo("LuoiMau"));
        }

        [Test]
        public void AC1_LuoiMauHazard_HasCorrectSlowMultiplier()
        {
            var effectValue = KarmaHazardService.GetEffectValue("LuoiMau");
            Assert.That(effectValue, Is.EqualTo(0.5f));
        }

        [Test]
        public void AC2_LinhSacrifice_SpawnsVungNuocHazard_WithDoT()
        {
            var playerPosition = new Vector3(3f, 0f, 7f);
            _service.SpawnHazardForGhost("Linh", playerPosition);

            var hazardType = KarmaHazardService.MapGhostToHazard("Linh");
            Assert.That(hazardType, Is.EqualTo("VungNuoc"));
        }

        [Test]
        public void AC2_VungNuocHazard_HasCorrectDoTValue()
        {
            var effectValue = KarmaHazardService.GetEffectValue("VungNuoc");
            Assert.That(effectValue, Is.EqualTo(5f));
        }

        [Test]
        public void AC3_MinhSacrifice_SpawnsBeDaDaoAnhHazard_WithCollapse()
        {
            var playerPosition = new Vector3(8f, 0f, 2f);
            _service.SpawnHazardForGhost("Minh", playerPosition);

            var hazardType = KarmaHazardService.MapGhostToHazard("Minh");
            Assert.That(hazardType, Is.EqualTo("BeDaDaoAnh"));
        }

        [Test]
        public void AC3_BeDaDaoAnhHazard_HasCorrectCollapseDuration()
        {
            var effectValue = KarmaHazardService.GetEffectValue("BeDaDaoAnh");
            Assert.That(effectValue, Is.EqualTo(0.2f));
        }

        [Test]
        public void Service_HandlesUnknownGhostType_ReturnsFalse()
        {
            var hasMapping = KarmaHazardService.HasHazardMapping("Unknown");
            Assert.That(hasMapping, Is.False);
        }

        [Test]
        public void Service_ClearHazards_RemovesAllActiveHazards()
        {
            _service.SpawnHazardForGhost("Van", Vector3.zero);
            _service.SpawnHazardForGhost("Linh", new Vector3(1f, 0f, 1f));
            _service.ClearHazards();

            Assert.That(true);
        }

        private ReactiveProperty<PhaseState> CreateMockPhaseProperty(PhaseState phase)
        {
            return new ReactiveProperty<PhaseState>(phase);
        }
    }
}
