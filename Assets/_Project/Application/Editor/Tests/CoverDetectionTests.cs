// Assets/_Project/Application/Editor/Tests/CoverDetectionTests.cs
using NUnit.Framework;
using R3;
using UnityEngine;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Tests
{
    /// <summary>
    /// Validates: TR-player-004 — Cover Detection — Full Containment Validation.
    /// Story 004: Cover Detection.
    ///
    /// Tests CoverDetector bounds math in isolation.
    /// No Collider, no CharacterController, no scene required.
    /// </summary>
    [TestFixture]
    public class CoverDetectionTests
    {
        private CoverDetector _detector;

        // ── Helpers ────────────────────────────────────────────────

        /// <summary>Creates a Bounds centred at origin with given half-extents.</summary>
        private static Bounds B(float halfX, float halfY, float halfZ)
            => new Bounds(Vector3.zero, new Vector3(halfX * 2f, halfY * 2f, halfZ * 2f));

        /// <summary>Creates a Bounds centred at <paramref name="centre"/> with given half-extents.</summary>
        private static Bounds B(Vector3 centre, float halfX, float halfY, float halfZ)
            => new Bounds(centre, new Vector3(halfX * 2f, halfY * 2f, halfZ * 2f));

        [SetUp]
        public void Setup()
        {
            _detector = new CoverDetector();
        }

        // ── AC-1: Full containment → IsInCover = true ─────────────

        [Test]
        public void AC1_PlayerFullyInsideVolume_IsInCover_True()
        {
            // Player: 1×1×1 cube at origin
            // Volume: 3×3×3 cube at origin — player fully inside
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.IsTrue(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC1_PlayerExactlyFitsVolume_IsInCover_True()
        {
            // Player bounds == volume bounds — exact fit counts as fully inside
            var bounds = B(1f, 1f, 1f);

            _detector.CheckCover(bounds, new[] { bounds }, PlayerInputMode.NightMovement);

            Assert.IsTrue(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC1_MultipleVolumes_PlayerInsideOne_IsInCover_True()
        {
            var player  = B(0.5f, 0.5f, 0.5f);
            var volFar  = B(new Vector3(100f, 0f, 0f), 0.5f, 0.5f, 0.5f); // far away
            var volNear = B(1.5f, 1.5f, 1.5f);                             // contains player

            _detector.CheckCover(player, new[] { volFar, volNear }, PlayerInputMode.NightMovement);

            Assert.IsTrue(_detector.IsInCover.CurrentValue);
        }

        // ── AC-2: Partial overlap → IsInCover = false ─────────────

        [Test]
        public void AC2_PlayerPartiallyOutside_IsInCover_False()
        {
            // Player: 1×1×1 at origin (extends from -0.5 to +0.5)
            // Volume: 1×1×1 offset so player is 50% outside on X
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(new Vector3(0.5f, 0f, 0f), 0.5f, 0.5f, 0.5f);
            // volume: x from 0 to 1 — player x from -0.5 to 0.5 → partial overlap

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.IsFalse(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC2_PlayerLargerThanVolume_IsInCover_False()
        {
            // Player bigger than volume — cannot be contained
            var player = B(2f, 2f, 2f);
            var volume = B(1f, 1f, 1f);

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.IsFalse(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC2_NoVolumes_IsInCover_False()
        {
            var player = B(0.5f, 0.5f, 0.5f);

            _detector.CheckCover(player, new Bounds[0], PlayerInputMode.NightMovement);

            Assert.IsFalse(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC2_NullVolumes_IsInCover_False()
        {
            var player = B(0.5f, 0.5f, 0.5f);

            _detector.CheckCover(player, null, PlayerInputMode.NightMovement);

            Assert.IsFalse(_detector.IsInCover.CurrentValue);
        }

        [Test]
        public void AC2_PlayerJustOutsideOnOneAxis_IsInCover_False()
        {
            // Player extends 0.01 units beyond volume on Z axis
            var player = B(0.5f, 0.5f, 0.51f); // z half-extent slightly larger
            var volume = B(1.5f, 1.5f, 0.5f);  // z half-extent = 0.5

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.IsFalse(_detector.IsInCover.CurrentValue,
                "Even 0.01 units outside must count as exposed");
        }

        // ── AC-3: Cover check disabled outside NightSurvival ──────

        [Test]
        public void AC3_DayUI_CheckCover_DoesNotUpdateState()
        {
            // Pre-condition: IsInCover = false
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f); // would contain player

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.DayUI);

            Assert.IsFalse(_detector.IsInCover.CurrentValue,
                "Cover check must not run in DayUI mode");
        }

        [Test]
        public void AC3_Disabled_CheckCover_DoesNotUpdateState()
        {
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.Disabled);

            Assert.IsFalse(_detector.IsInCover.CurrentValue,
                "Cover check must not run in Disabled mode");
        }

        [Test]
        public void AC3_DayUI_DoesNotClearExistingCoverState()
        {
            // Enter cover in NightMovement
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);
            Assert.IsTrue(_detector.IsInCover.CurrentValue);

            // Phase changes to DayUI — cover state must not be cleared
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.DayUI);

            Assert.IsTrue(_detector.IsInCover.CurrentValue,
                "Cover state must not change when check is skipped");
        }

        // ── AC-4: ReactiveProperty fires on state change only ──────

        [Test]
        public void AC4_EnterCover_FiresOnce()
        {
            var events = new System.Collections.Generic.List<bool>();
            _detector.IsInCover.Subscribe(v => events.Add(v));
            int baseline = events.Count; // initial subscription fires once

            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.AreEqual(baseline + 1, events.Count, "Should fire exactly once on enter");
            Assert.IsTrue(events[events.Count - 1]);
        }

        [Test]
        public void AC4_StayInCover_NoRepeatFire()
        {
            var events = new System.Collections.Generic.List<bool>();
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            // Enter cover
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            _detector.IsInCover.Subscribe(v => events.Add(v));
            int baseline = events.Count;

            // Stay inside — multiple ticks
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.AreEqual(baseline, events.Count,
                "No additional events while staying inside cover");
        }

        [Test]
        public void AC4_ExitCover_FiresFalse()
        {
            var events = new System.Collections.Generic.List<bool>();
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            // Enter
            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);
            _detector.IsInCover.Subscribe(v => events.Add(v));
            int baseline = events.Count;

            // Exit — player moves outside
            var bigPlayer = B(2f, 2f, 2f);
            _detector.CheckCover(bigPlayer, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.AreEqual(baseline + 1, events.Count);
            Assert.IsFalse(events[events.Count - 1]);
        }

        // ── cover_enter_threshold configurable ────────────────────

        [Test]
        public void Threshold_Default_Is1d0()
        {
            Assert.AreEqual(CoverDetector.DefaultThreshold, _detector.CoverEnterThreshold);
        }

        [Test]
        public void Threshold_0d9_AllowsSlightOverhang()
        {
            _detector.CoverEnterThreshold = 0.9f;

            // Player: 1×1×1 (half=0.5), volume: 1×1×1 (half=0.5)
            // At threshold=0.9, shrink = 0.5 * 0.1 = 0.05 on each side
            // Shrunk player: half=0.45 — fits inside volume half=0.5
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(0.5f, 0.5f, 0.5f);

            _detector.CheckCover(player, new[] { volume }, PlayerInputMode.NightMovement);

            Assert.IsTrue(_detector.IsInCover.CurrentValue,
                "At threshold=0.9, player that slightly overhangs should still count as covered");
        }

        [Test]
        public void Threshold_BelowMin_ClampedTo0d8()
        {
            _detector.CoverEnterThreshold = 0.1f;

            Assert.AreEqual(CoverDetector.MinThreshold, _detector.CoverEnterThreshold);
        }

        [Test]
        public void Threshold_AboveMax_ClampedTo1d0()
        {
            _detector.CoverEnterThreshold = 2.0f;

            Assert.AreEqual(CoverDetector.MaxThreshold, _detector.CoverEnterThreshold);
        }

        // ── IsContainedWithinThreshold static helper ──────────────

        [Test]
        public void StaticHelper_FullContainment_ReturnsTrue()
        {
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(1.5f, 1.5f, 1.5f);

            Assert.IsTrue(CoverDetector.IsContainedWithinThreshold(player, volume, 1.0f));
        }

        [Test]
        public void StaticHelper_PartialOverlap_ReturnsFalse()
        {
            var player = B(0.5f, 0.5f, 0.5f);
            var volume = B(new Vector3(0.5f, 0f, 0f), 0.5f, 0.5f, 0.5f);

            Assert.IsFalse(CoverDetector.IsContainedWithinThreshold(player, volume, 1.0f));
        }
    }
}
