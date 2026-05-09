// Assets/_Project/Application/Editor/Tests/WasdMovementTests.cs
using NUnit.Framework;
using UnityEngine;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Tests
{
    /// <summary>
    /// Validates: TR-player-002 — WASD Base Movement.
    /// Story 002: WASD Base Movement — CharacterController Night Traversal.
    ///
    /// Tests the MovementCalculator formula in isolation:
    ///   displacement = (input.xz * baseMoveSpeed + gravity) * deltaTime
    /// No CharacterController, no scene, no Unity physics required.
    /// </summary>
    [TestFixture]
    public class WasdMovementTests
    {
        private MovementCalculator _calc;

        // Fixed deltaTime for deterministic tests (1 second = easy math)
        private const float DeltaTime1s  = 1.0f;
        private const float DeltaTime0p5 = 0.5f;

        [SetUp]
        public void Setup()
        {
            _calc = new MovementCalculator();
            // Default speed = 5.0
        }

        // ── AC-1: WASD moves at base speed ────────────────────────

        [Test]
        public void AC1_ForwardInput_ProducesPositiveZ_AtBaseSpeed()
        {
            // W key = (0, 1) input, 1 second, NightMovement
            var result = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            // Horizontal Z = 1.0 * 5.0 * 1.0 = 5.0
            Assert.AreEqual(5.0f, result.z, 0.001f, "Forward (W) should produce +Z displacement of 5 units/s");
        }

        [Test]
        public void AC1_BackwardInput_ProducesNegativeZ_AtBaseSpeed()
        {
            var result = _calc.CalculateMovement(
                new Vector2(0f, -1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(-5.0f, result.z, 0.001f, "Backward (S) should produce -Z displacement");
        }

        [Test]
        public void AC1_RightInput_ProducesPositiveX_AtBaseSpeed()
        {
            var result = _calc.CalculateMovement(
                new Vector2(1f, 0f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(5.0f, result.x, 0.001f, "Right (D) should produce +X displacement");
        }

        [Test]
        public void AC1_LeftInput_ProducesNegativeX_AtBaseSpeed()
        {
            var result = _calc.CalculateMovement(
                new Vector2(-1f, 0f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(-5.0f, result.x, 0.001f, "Left (A) should produce -X displacement");
        }

        [Test]
        public void AC1_ZeroInput_ProducesZeroHorizontal()
        {
            var result = _calc.CalculateMovement(
                Vector2.zero,
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(0f, result.x, 0.001f);
            Assert.AreEqual(0f, result.z, 0.001f);
        }

        // ── AC-2: Movement blocked outside NightMovement ──────────

        [Test]
        public void AC2_DayUI_ReturnsZeroVector()
        {
            var result = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime1s,
                PlayerInputMode.DayUI
            );

            Assert.AreEqual(Vector3.zero, result, "DayUI mode must produce zero displacement");
        }

        [Test]
        public void AC2_Disabled_ReturnsZeroVector()
        {
            var result = _calc.CalculateMovement(
                new Vector2(1f, 1f),
                DeltaTime1s,
                PlayerInputMode.Disabled
            );

            Assert.AreEqual(Vector3.zero, result, "Disabled mode must produce zero displacement");
        }

        // ── AC-3: Gravity applied each frame ──────────────────────

        [Test]
        public void AC3_Gravity_ProducesNegativeY_EachFrame()
        {
            var result = _calc.CalculateMovement(
                Vector2.zero,
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            // gravity = -9.81 * 1.0 = -9.81
            Assert.AreEqual(-MovementCalculator.GravityAcceleration, result.y, 0.001f,
                "Gravity must apply -9.81 * deltaTime to Y each frame");
        }

        [Test]
        public void AC3_Gravity_ScalesWithDeltaTime()
        {
            var result = _calc.CalculateMovement(
                Vector2.zero,
                DeltaTime0p5,
                PlayerInputMode.NightMovement
            );

            float expected = -MovementCalculator.GravityAcceleration * DeltaTime0p5;
            Assert.AreEqual(expected, result.y, 0.001f,
                "Gravity must scale proportionally with deltaTime");
        }

        [Test]
        public void AC3_Gravity_AppliedEvenWithNoHorizontalInput()
        {
            var result = _calc.CalculateMovement(
                Vector2.zero,
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.Less(result.y, 0f, "Y component must be negative (downward) even with no WASD input");
        }

        [Test]
        public void AC3_Gravity_NotApplied_WhenModeIsDisabled()
        {
            var result = _calc.CalculateMovement(
                Vector2.zero,
                DeltaTime1s,
                PlayerInputMode.Disabled
            );

            Assert.AreEqual(0f, result.y, 0.001f,
                "Gravity must not apply when mode is Disabled");
        }

        // ── AC-4: base_move_speed is configurable ─────────────────

        [Test]
        public void AC4_CustomSpeed_3_ProducesCorrectDisplacement()
        {
            _calc.BaseMoveSpeed = 3.0f;

            var result = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(3.0f, result.z, 0.001f,
                "With baseMoveSpeed=3.0, forward displacement should be 3.0 units/s");
        }

        [Test]
        public void AC4_CustomSpeed_8_ProducesCorrectDisplacement()
        {
            _calc.BaseMoveSpeed = 8.0f;

            var result = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(8.0f, result.z, 0.001f);
        }

        [Test]
        public void AC4_DefaultSpeed_Is5()
        {
            Assert.AreEqual(MovementCalculator.DefaultBaseMoveSpeed, _calc.BaseMoveSpeed);
        }

        [Test]
        public void AC4_SpeedBelowMin_ClampedTo2()
        {
            _calc.BaseMoveSpeed = 0.5f;

            Assert.AreEqual(MovementCalculator.MinBaseMoveSpeed, _calc.BaseMoveSpeed,
                "Speed below minimum must be clamped to 2.0");
        }

        [Test]
        public void AC4_SpeedAboveMax_ClampedTo8()
        {
            _calc.BaseMoveSpeed = 99f;

            Assert.AreEqual(MovementCalculator.MaxBaseMoveSpeed, _calc.BaseMoveSpeed,
                "Speed above maximum must be clamped to 8.0");
        }

        // ── Diagonal movement ──────────────────────────────────────

        [Test]
        public void DiagonalInput_ProducesCorrectXZComponents()
        {
            // Input (1, 1) — not normalised, raw diagonal
            var result = _calc.CalculateMovement(
                new Vector2(1f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(5.0f, result.x, 0.001f);
            Assert.AreEqual(5.0f, result.z, 0.001f);
        }

        // ── DeltaTime scaling ──────────────────────────────────────

        [Test]
        public void HalfDeltaTime_ProducesHalfDisplacement()
        {
            var full = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            var half = _calc.CalculateMovement(
                new Vector2(0f, 1f),
                DeltaTime0p5,
                PlayerInputMode.NightMovement
            );

            Assert.AreEqual(full.z * 0.5f, half.z, 0.001f,
                "Displacement must scale linearly with deltaTime");
        }

        // ── Y axis is gravity only (no vertical input) ─────────────

        [Test]
        public void HorizontalInput_DoesNotAffectY()
        {
            var result = _calc.CalculateMovement(
                new Vector2(1f, 1f),
                DeltaTime1s,
                PlayerInputMode.NightMovement
            );

            float expectedY = -MovementCalculator.GravityAcceleration * DeltaTime1s;
            Assert.AreEqual(expectedY, result.y, 0.001f,
                "Y must only contain gravity — horizontal input must not affect Y");
        }
    }
}
