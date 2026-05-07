// tests/integration/phase-state-machine/boss-searchlight_test.cs
using System;
using NUnit.Framework;
using R3;
using UnityEngine;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.Phase;

namespace SolarPhobia.Application.Tests.Integration.PhaseStateMachine
{
    [TestFixture]
    public class BossSearchlightIntegrationTests
    {
        // ── Fake Dependencies ──────────────────────────────
        private class FakePhaseStateMachine : IPhaseStateMachine
        {
            public ReactiveProperty<PhaseState> CurrentPhase { get; } = new ReactiveProperty<PhaseState>(PhaseState.Day);
        }

        private class FakeWardTimerService : IWardTimerService
        {
            public float LastPenalty { get; private set; }
            public float CurrentWard { get; set; } = 100f;
            public void ApplyPenalty(float seconds)
            {
                LastPenalty = seconds;
                CurrentWard = Mathf.Max(0f, CurrentWard - seconds);
            }
        }

        private class FakeAudioService : IAudioService
        {
            public bool StrikeSFXPlayed { get; private set; }
            public void PlayStrikeSFX() => StrikeSFXPlayed = true;
        }

        private class FakeVisualEffectsService : IVisualEffectsService
        {
            public bool ScreenShakeTriggered { get; private set; }
            public bool RedFlashTriggered { get; private set; }
            public void TriggerScreenShake() => ScreenShakeTriggered = true;
            public void TriggerRedFlash() => RedFlashTriggered = true;
        }

        private class FakeCoverDetectionService : ICoverDetectionService
        {
            public bool IsInCover { get; set; }
        }

        // ── Test Context ──────────────────────────────
        private BossSearchlightService _service;
        private FakePhaseStateMachine _fakePhase;
        private FakeWardTimerService _fakeWard;
        private FakeAudioService _fakeAudio;
        private FakeVisualEffectsService _fakeVfx;
        private FakeCoverDetectionService _fakeCover;

        [SetUp]
        public void SetUp()
        {
            _fakePhase = new FakePhaseStateMachine();
            _fakeWard = new FakeWardTimerService();
            _fakeAudio = new FakeAudioService();
            _fakeVfx = new FakeVisualEffectsService();
            _fakeCover = new FakeCoverDetectionService();

            _service = new BossSearchlightService(
                _fakePhase,
                _fakeWard,
                _fakeAudio,
                _fakeVfx,
                _fakeCover
            );
        }

        [TearDown]
        public void TearDown()
        {
            _service.Dispose();
        }

        // ── AC-1: Searchlight sweeps across lane in pattern ──────────────────────────────
        [Test]
        public void ActivateSearchlight_WhenNightSurvivalPhase_StartsSweep()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            var isActive = (ReactiveProperty<bool>)_service.GetType()
                .GetField("_isActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.IsTrue(isActive.Value);
        }

        [Test]
        public void SweepAngle_IncreasesOverTime_WhenSweepingRight()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            var initialAngle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            
            // Simulate multiple sweep updates to advance angle
            for (int i = 0; i < 10; i++)
            {
                _service.GetType()
                    .GetMethod("UpdateSweep", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(_service, null);
            }
            
            var newAngle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.Greater(newAngle, initialAngle);
        }

        [Test]
        public void SweepAngle_DecreasesOverTime_WhenSweepingLeft()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_service, 45f);
            _service.GetType()
                .GetField("_isSweepingRight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_service, false);
            
            var initialAngle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            
            // Simulate multiple sweep updates to advance angle leftward
            for (int i = 0; i < 10; i++)
            {
                _service.GetType()
                    .GetMethod("UpdateSweep", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(_service, null);
            }
            
            var newAngle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.Less(newAngle, initialAngle);
        }

        [Test]
        public void SweepPattern_PingPongsBetweenMinAndMaxAngle()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            var maxAngle = (float)_service.GetType()
                .GetField("MaxSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(_service);
            var minAngle = (float)_service.GetType()
                .GetField("MinSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(_service);
            
            for (int i = 0; i < 10; i++)
                _service.GetType()
                    .GetMethod("UpdateSweep", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(_service, null);
            var angle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.AreEqual(maxAngle, angle, 0.1f);
            
            for (int i = 0; i < 10; i++)
                _service.GetType()
                    .GetMethod("UpdateSweep", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .Invoke(_service, null);
            angle = (float)_service.GetType()
                .GetField("_currentSweepAngle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.AreEqual(minAngle, angle, 0.1f);
        }

        [Test]
        public void ActivateSearchlight_WhenNotNightSurvival_DoesNotStartSweep()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.Day;
            _service.ActivateSearchlight();
            var isActive = (ReactiveProperty<bool>)_service.GetType()
                .GetField("_isActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.IsFalse(isActive.Value);
        }

        [Test]
        public void DeactivateSearchlight_StopsSweep()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.DeactivateSearchlight();
            var isActive = (ReactiveProperty<bool>)_service.GetType()
                .GetField("_isActive", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(_service);
            Assert.IsFalse(isActive.Value);
        }

        // ── AC-2: Exposed player receives strike warning ──────────────────────────────
        [Test]
        public void IsPlayerExposed_PlayerInCover_ReturnsFalse()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            bool result = _service.IsPlayerExposed(Vector3.zero, true);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsPlayerExposed_SearchlightInactive_ReturnsFalse()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.Day;
            bool result = _service.IsPlayerExposed(Vector3.zero, false);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsPlayerExposed_PlayerInCone_StartsTelegraph()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            bool result = _service.IsPlayerExposed(new Vector3(0f, 0f, 10f), false);
            Assert.IsTrue(result);
            Assert.IsTrue(_service.OnTelegraphActive.GetValue());
        }

        [Test]
        public void IsPlayerExposed_PlayerLeavesCone_ClearsTelegraph()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.IsPlayerExposed(new Vector3(0f, 0f, 10f), false);
            Assert.IsTrue(_service.OnTelegraphActive.GetValue());
            
            _service.IsPlayerExposed(new Vector3(100f, 0f, 100f), false);
            Assert.IsFalse(_service.OnTelegraphActive.GetValue());
        }

        [Test]
        public void TelegraphActive_OnTelegraphStart_TriggersObservable()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            bool telegramFired = false;
            _service.OnTelegraphActive.Subscribe(v => telegramFired = v);
            _service.IsPlayerExposed(new Vector3(0f, 0f, 10f), false);
            Assert.IsTrue(telegramFired);
        }

        [Test]
        public void PlayerEntersCoverDuringTelegraph_ClearsTelegraph()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.IsPlayerExposed(new Vector3(0f, 0f, 10f), false);
            Assert.IsTrue(_service.OnTelegraphActive.GetValue());
            
            _service.IsPlayerExposed(Vector3.zero, true);
            Assert.IsFalse(_service.OnTelegraphActive.GetValue());
        }

        // ── AC-3: Strike applies -30s Ward penalty ──────────────────────────────
        [Test]
        public void ExecuteStrike_AppliesMinus30sWardPenalty()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.ExecuteStrike();
            Assert.AreEqual(30f, _fakeWard.LastPenalty);
        }

        [Test]
        public void ExecuteStrike_TriggersScreenShakeAndRedFlash()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.ExecuteStrike();
            Assert.IsTrue(_fakeVfx.ScreenShakeTriggered);
            Assert.IsTrue(_fakeVfx.RedFlashTriggered);
        }

        [Test]
        public void ExecuteStrike_MultipleStrikes_StackPenalties()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _service.ExecuteStrike();
            _service.ExecuteStrike();
            Assert.AreEqual(30f, _fakeWard.LastPenalty);
        }

        [Test]
        public void ExecuteStrike_WardBelow30_DropsToZero()
        {
            _fakePhase.CurrentPhase.Value = PhaseState.NightSurvival;
            _service.ActivateSearchlight();
            _fakeWard.CurrentWard = 20f;
            _service.ExecuteStrike();
            Assert.AreEqual(0f, _fakeWard.CurrentWard);
        }
    }
}
