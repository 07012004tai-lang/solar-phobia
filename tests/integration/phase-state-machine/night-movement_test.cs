using System;
using NUnit.Framework;
using UnityEngine;
using R3;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Tests.Integration.PhaseStateMachine
{
    /// <summary>
    /// Integration tests for Night Phase Movement Service.
    /// Tests WASD movement, Sprint, Dash, Swing, Glide with Ward timer penalties.
    /// Validates TR-state-002: Day/Night phase transitions with phase-gated system activation.
    /// </summary>
    [TestFixture]
    public class NightMovementIntegrationTests
    {
        private FakeWardTimerService _wardTimer;
        private FakeAudioService _audioService;
        private FakeCharacterController _characterController;
        private NightPhaseMovementService _service;
        
        [SetUp]
        public void SetUp()
        {
            _wardTimer = new FakeWardTimerService(100f);
            _audioService = new FakeAudioService();
            _characterController = new FakeCharacterController();
            
            _service = new NightPhaseMovementService(
                _characterController,
                _wardTimer,
                _audioService
            );
        }
        
        // ── AC-1: WASD Movement Phase Gating ──────────────────────────────
        
        [Test]
        public void AC1_WASD_Movement_OnlyWorksInNightSurvival()
        {
            // Given: CurrentPhase is DayService
            var dayPhase = PhaseState.DayService;
            var inputDirection = new Vector3(1f, 0f, 0f);
            
            // When: Player presses WASD keys
            var result = _service.TryMove(inputDirection, dayPhase);
            
            // Then: No movement occurs (action rejected by phase contract)
            Assert.That(result, Is.False);
            Assert.That(_characterController.MoveCallCount, Is.EqualTo(0));
        }
        
        [Test]
        public void AC1_WASD_Movement_EnablesAfterPhaseTransition()
        {
            // Given: CurrentPhase is NightSurvival
            var nightPhase = PhaseState.NightSurvival;
            var inputDirection = new Vector3(1f, 0f, 0f);
            
            // When: Player presses WASD keys
            var result = _service.TryMove(inputDirection, nightPhase);
            
            // Then: Movement occurs
            Assert.That(result, Is.True);
            Assert.That(_characterController.MoveCallCount, Is.GreaterThan(0));
        }
        
        // ── AC-2: Sprint Ward Penalty ────────────────────────────────────
        
        [Test]
        public void AC2_Sprint_AppliesWardPenalty()
        {
            // Given: CurrentPhase is NightSurvival, player is moving
            var nightPhase = PhaseState.NightSurvival;
            
            // When: Player holds Shift to sprint
            var result = _service.TrySprint(nightPhase);
            
            // Then: Sprint activates AND Ward timer decreases by sprint cost (5s)
            Assert.That(result, Is.True);
            Assert.That(_wardTimer.TotalCostApplied, Is.EqualTo(5.0f));
            Assert.That(_audioService.SprintSoundPlayed, Is.True);
        }
        
        [Test]
        public void AC2_Sprint_DisabledWhenWardZero()
        {
            // Given: Ward timer at 0
            _wardTimer = new FakeWardTimerService(0f);
            _service = new NightPhaseMovementService(_characterController, _wardTimer, _audioService);
            var nightPhase = PhaseState.NightSurvival;
            
            // When: Player holds Shift to sprint
            var result = _service.TrySprint(nightPhase);
            
            // Then: Sprint fails (insufficient Ward)
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void AC2_Sprint_NotAllowedInDayService()
        {
            // Given: CurrentPhase is DayService
            var dayPhase = PhaseState.DayService;
            
            // When: Player holds Shift to sprint
            var result = _service.TrySprint(dayPhase);
            
            // Then: Sprint rejected by phase contract
            Assert.That(result, Is.False);
            Assert.That(_wardTimer.TotalCostApplied, Is.EqualTo(0f));
        }
        
        // ── AC-3: Spirit Dash ──────────────────────────────────────────────
        
        [Test]
        public void AC3_Dash_Applies5sWardPenalty()
        {
            // Given: CurrentPhase is NightSurvival
            var nightPhase = PhaseState.NightSurvival;
            var direction = Vector3.forward;
            
            // When: Player activates Spirit Dash
            var result = _service.TryDash(direction, nightPhase);
            
            // Then: Quick forward burst AND Ward decreases by 5 seconds
            Assert.That(result, Is.True);
            Assert.That(_wardTimer.TotalCostApplied, Is.EqualTo(5.0f));
            Assert.That(_characterController.MoveCallCount, Is.GreaterThan(0));
            Assert.That(_audioService.DashSoundPlayed, Is.True);
        }
        
        [Test]
        public void AC3_Dash_DisabledWhenWardLessThan5s()
        {
            // Given: Ward < 5s
            _wardTimer = new FakeWardTimerService(3f);
            _service = new NightPhaseMovementService(_characterController, _wardTimer, _audioService);
            var nightPhase = PhaseState.NightSurvival;
            var direction = Vector3.forward;
            
            // When: Player activates Spirit Dash
            var result = _service.TryDash(direction, nightPhase);
            
            // Then: Dash fails (insufficient Ward)
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void AC3_Dash_NotAllowedInDayService()
        {
            // Given: CurrentPhase is DayService
            var dayPhase = PhaseState.DayService;
            var direction = Vector3.forward;
            
            // When: Player activates Spirit Dash
            var result = _service.TryDash(direction, dayPhase);
            
            // Then: Dash rejected by phase contract
            Assert.That(result, Is.False);
        }
        
        // ── AC-4: Swing ──────────────────────────────────────────────────
        
        [Test]
        public void AC4_Swing_Applies2sWardPenalty()
        {
            // Given: CurrentPhase is NightSurvival
            var nightPhase = PhaseState.NightSurvival;
            var targetPoint = new Vector3(0f, 0f, 15f);
            
            // When: Player activates Swing (rope/grapple)
            var result = _service.TrySwing(targetPoint, nightPhase);
            
            // Then: Forward swing motion AND Ward decreases by 2 seconds
            Assert.That(result, Is.True);
            Assert.That(_wardTimer.TotalCostApplied, Is.EqualTo(2.0f));
            Assert.That(_characterController.MoveCallCount, Is.GreaterThan(0));
            Assert.That(_audioService.SwingSoundPlayed, Is.True);
        }
        
        [Test]
        public void AC4_Swing_NotAllowedInDayService()
        {
            // Given: CurrentPhase is DayService
            var dayPhase = PhaseState.DayService;
            var targetPoint = Vector3.forward;
            
            // When: Player activates Swing
            var result = _service.TrySwing(targetPoint, dayPhase);
            
            // Then: Swing rejected by phase contract
            Assert.That(result, Is.False);
        }
        
        // ── AC-5: Glide ──────────────────────────────────────────────────
        
        [Test]
        public void AC5_Glide_Applies1sPerSecWardPenalty()
        {
            // Given: CurrentPhase is NightSurvival, player is airborne
            var nightPhase = PhaseState.NightSurvival;
            
            // When: Player holds jump to glide
            var result = _service.TryGlide(true, nightPhase);
            
            // Then: Glide activates
            Assert.That(result, Is.True);
            Assert.That(_wardTimer.TotalCostApplied, Is.GreaterThan(0f));
        }
        
        [Test]
        public void AC5_Glide_StopsWhenGroundContact()
        {
            // Given: Player was gliding
            var nightPhase = PhaseState.NightSurvival;
            _service.TryGlide(true, nightPhase);
            
            // When: Ground contact (glide stops)
            var result = _service.TryGlide(false, nightPhase);
            
            // Then: Glide stops
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void AC5_Glide_StopsWhenWardReachesZero()
        {
            // Given: Ward reaches 0
            _wardTimer = new FakeWardTimerService(0.5f);
            _service = new NightPhaseMovementService(_characterController, _wardTimer, _audioService);
            var nightPhase = PhaseState.NightSurvival;
            
            // When: Player holds jump to glide
            var result = _service.TryGlide(true, nightPhase);
            
            // Then: Glide stops (insufficient Ward)
            Assert.That(result, Is.False);
        }
        
        [Test]
        public void AC5_Glide_NotAllowedInDayService()
        {
            // Given: CurrentPhase is DayService
            var dayPhase = PhaseState.DayService;
            
            // When: Player tries to glide
            var result = _service.TryGlide(true, dayPhase);
            
            // Then: Glide rejected by phase contract
            Assert.That(result, Is.False);
        }
        
        // ── Ward Timer Observable Tests ────────────────────────────────────
        
        [Test]
        public void WardChanged_Observable_NotifiesOnChange()
        {
            // Given: Subscribed to Ward changes
            bool notified = false;
            var subscription = _service.OnWardChanged.Subscribe(v => notified = true);
            
            // When: Any action that changes Ward occurs
            _service.TrySprint(PhaseState.NightSurvival);
            
            // Then: Observable fired
            Assert.That(notified, Is.True);
            
            subscription.Dispose();
        }
    }
    
    // ── Fake Implementations for Testing ────────────────────────────────
    
    public class FakeWardTimerService : IWardTimerService
    {
        private float _currentWard;
        public float TotalCostApplied { get; private set; }
        
        public float CurrentWard
        {
            get => _currentWard;
            set => _currentWard = value;
        }
        
        public FakeWardTimerService(float initialWard)
        {
            _currentWard = initialWard;
        }
        
        public float GetCurrentWard() => _currentWard;
        
        public bool TryApplyCost(float cost)
        {
            if (_currentWard >= cost)
            {
                _currentWard -= cost;
                TotalCostApplied += cost;
                return true;
            }
            return false;
        }
        
        private readonly Subject<float> _wardChangedSubject = new Subject<float>();
        public Observable<float> OnWardChanged => _wardChangedSubject;
    }
    
    public class FakeAudioService : IAudioService
    {
        public bool SprintSoundPlayed { get; private set; }
        public bool DashSoundPlayed { get; private set; }
        public bool SwingSoundPlayed { get; private set; }
        public bool SwapSoundPlayed { get; private set; }
        public bool ShoveImpactPlayed { get; private set; }
        public bool SoulBurnPlayed { get; private set; }
        
        public void PlaySprintSound() => SprintSoundPlayed = true;
        public void PlayDashSound() => DashSoundPlayed = true;
        public void PlaySwingSound() => SwingSoundPlayed = true;
        public void PlaySwapSound() => SwapSoundPlayed = true;
        public void PlayShoveImpact() => ShoveImpactPlayed = true;
        public void PlaySoulBurn() => SoulBurnPlayed = true;
    }
    
    public class FakeCharacterController
    {
        public int MoveCallCount { get; private set; }
        public Vector3 LastMove { get; private set; }
        
        public void Move(Vector3 move)
        {
            MoveCallCount++;
            LastMove = move;
        }
    }
}
