using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using R3;
using SolarPhobia.Application.Services;

namespace SolarPhobia.Application.Tests.Integration.PhaseStateMachine
{
    [TestFixture]
    public class CoverDetectionIntegrationTests
    {
        private CoverDetectionService _service;
        private ReactiveProperty<PhaseState> _phaseProperty;

        [SetUp]
        public void SetUp()
        {
            _phaseProperty = new ReactiveProperty<PhaseState>(PhaseState.NightSurvival);
            _service = new CoverDetectionService(_phaseProperty);
        }

        [TearDown]
        public void TearDown()
        {
            _phaseProperty?.Dispose();
        }

        // ── AC-1: Full collider inside Mound = valid cover ──────────────────────────────
        [Test]
        public void Test_IsFullyInsideMound_PlayerFullyInside_ReturnsTrue()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(2, 2, 2);
            playerCollider.center = Vector3.zero;

            // Act
            bool result = _service.IsFullyInsideMound(playerCollider, moundCollider);

            // Assert
            Assert.That(result, Is.True);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsFullyInsideMound_PlayerPartiallyInside_ReturnsFalse()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(10, 10, 10);
            playerCollider.center = new Vector3(5, 0, 0);

            // Act
            bool result = _service.IsFullyInsideMound(playerCollider, moundCollider);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsFullyInsideMound_PlayerOutside_ReturnsFalse()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(2, 2, 2);
            playerCollider.center = new Vector3(20, 0, 0);

            // Act
            bool result = _service.IsFullyInsideMound(playerCollider, moundCollider);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsFullyInsideMound_NullPlayerCollider_ReturnsFalse()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();

            // Act
            bool result = _service.IsFullyInsideMound(null, moundCollider);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
        }

        [Test]
        public void Test_IsFullyInsideMound_NullMoundCollider_ReturnsFalse()
        {
            // Arrange
            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();

            // Act
            bool result = _service.IsFullyInsideMound(playerCollider, null);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsFullyInsideMound_EdgeOfMound_ReturnsFalse()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(2, 2, 2);
            playerCollider.center = new Vector3(5, 0, 0);

            // Act
            bool result = _service.IsFullyInsideMound(playerCollider, moundCollider);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        // ── AC-2: Partial collider outside Mound = exposed ──────────────────────────────
        [Test]
        public void Test_IsPlayerExposed_FullyInsideMound_ReturnsFalse()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(2, 2, 2);
            playerCollider.center = Vector3.zero;

            var mounds = new List<Collider> { moundCollider };

            // Act
            bool result = _service.IsPlayerExposed(playerCollider, mounds);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsPlayerExposed_PartiallyInsideMound_ReturnsTrue()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(10, 10, 10);
            playerCollider.center = new Vector3(5, 0, 0);

            var mounds = new List<Collider> { moundCollider };

            // Act
            bool result = _service.IsPlayerExposed(playerCollider, mounds);

            // Assert
            Assert.That(result, Is.True);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsPlayerExposed_OutsideAllMounds_ReturnsTrue()
        {
            // Arrange
            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            moundCollider.size = new Vector3(10, 10, 10);
            moundCollider.center = Vector3.zero;

            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            playerCollider.size = new Vector3(2, 2, 2);
            playerCollider.center = new Vector3(20, 0, 0);

            var mounds = new List<Collider> { moundCollider };

            // Act
            bool result = _service.IsPlayerExposed(playerCollider, mounds);

            // Assert
            Assert.That(result, Is.True);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsPlayerExposed_NoMounds_ReturnsTrue()
        {
            // Arrange
            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            var mounds = new List<Collider>();

            // Act
            bool result = _service.IsPlayerExposed(playerCollider, mounds);

            // Assert
            Assert.That(result, Is.True);

            // Cleanup
            Object.DestroyImmediate(playerGo);
        }

        [Test]
        public void Test_IsPlayerExposed_NullPlayerCollider_ReturnsFalse()
        {
            // Arrange
            var mounds = new List<Collider>();

            // Act
            bool result = _service.IsPlayerExposed(null, mounds);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Test_IsPlayerExposed_WrongPhase_ReturnsFalse()
        {
            // Arrange
            _phaseProperty.Value = PhaseState.Day;

            var moundGo = new GameObject("Mound");
            var moundCollider = moundGo.AddComponent<BoxCollider>();
            var playerGo = new GameObject("Player");
            var playerCollider = playerGo.AddComponent<BoxCollider>();
            var mounds = new List<Collider> { moundCollider };

            // Act
            bool result = _service.IsPlayerExposed(playerCollider, mounds);

            // Assert
            Assert.That(result, Is.False);

            // Cleanup
            Object.DestroyImmediate(moundGo);
            Object.DestroyImmediate(playerGo);
        }

        // ── AC-3: Different Mound types provide cover ──────────────────────────────
        [Test]
        public void Test_CheckMoundTypeCover_SafeMound_ReturnsTrue()
        {
            // Act
            bool result = _service.CheckMoundTypeCover("SafeMound");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Test_CheckMoundTypeCover_CursedMound_ReturnsTrue()
        {
            // Act
            bool result = _service.CheckMoundTypeCover("CursedMound");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Test_CheckMoundTypeCover_FalseSafeMound_ReturnsTrue()
        {
            // Act
            bool result = _service.CheckMoundTypeCover("FalseSafeMound");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Test_CheckMoundTypeCover_InvalidMound_ReturnsFalse()
        {
            // Act
            bool result = _service.CheckMoundTypeCover("InvalidMound");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}