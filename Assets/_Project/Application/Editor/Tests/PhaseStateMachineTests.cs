using System.Linq;
using NUnit.Framework;
using R3;
using SolarPhobia.Application.Messages;
using SolarPhobia.Application.Services;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Tests
{
    [TestFixture]
    public class PhaseStateMachineTests
    {
        private PhaseStateMachine _machine;

        [SetUp]
        public void SetUp()
        {
            _machine = new PhaseStateMachine();
            _machine.Initialize();
        }

        [Test]
        public void Initialize_StartsInDayService()
        {
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.DayService));
        }

        [Test]
        public void CurrentPhase_IsObservable()
        {
            bool observed = false;
            var sub = _machine.CurrentPhase.Subscribe(_ => observed = true);
            Assert.That(observed, Is.True);
            sub.Dispose();
        }

        [Test]
        public void TryTransition_FromDayServiceToChoiceLock_ReturnsTrue()
        {
            var result = _machine.TryTransition(PhaseState.ChoiceLock);
            Assert.That(result, Is.True);
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.ChoiceLock));
        }

        [Test]
        public void TryTransition_InvalidTransition_ReturnsFalse()
        {
            var result = _machine.TryTransition(PhaseState.NightSurvival);
            Assert.That(result, Is.False);
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.DayService));
        }

        [Test]
        public void TryTransition_EmitsPhaseChangedEvent()
        {
            PhaseChangedEvent? received = null;
            var sub = _machine.OnPhaseChanged.Subscribe(e => received = e);
            
            _machine.TryTransition(PhaseState.ChoiceLock);
            
            Assert.That(received, Is.Not.Null);
            Assert.That(received.Value.PreviousPhase, Is.EqualTo(PhaseState.DayService));
            Assert.That(received.Value.NewPhase, Is.EqualTo(PhaseState.ChoiceLock));
            sub.Dispose();
        }

        [Test]
        public void TryTransition_ToNightSurvival_EmitsNightStartEvent()
        {
            NightStartEvent? received = null;
            var sub = _machine.OnNightStart.Subscribe(e => received = e);
            
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            
            Assert.That(received, Is.Not.Null);
            sub.Dispose();
        }

        [Test]
        public void TryTransition_ToResolve_EmitsResolveEvent()
        {
            ResolveEvent? received = null;
            var sub = _machine.OnResolve.Subscribe(e => received = e);
            
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            _machine.TryTransition(PhaseState.Resolve);
            
            Assert.That(received, Is.Not.Null);
            sub.Dispose();
        }

        [Test]
        public void IsActionAllowed_DayService_ReturnsTrueForDayActions()
        {
            Assert.That(_machine.IsActionAllowed(GameAction.InspectSoul), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.AssignRitual), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.ConfirmSelection), Is.True);
        }

        [Test]
        public void IsActionAllowed_DayService_ReturnsFalseForNightActions()
        {
            Assert.That(_machine.IsActionAllowed(GameAction.Move), Is.False);
            Assert.That(_machine.IsActionAllowed(GameAction.Sprint), Is.False);
            Assert.That(_machine.IsActionAllowed(GameAction.Dash), Is.False);
        }

        [Test]
        public void IsActionAllowed_ChoiceLock_OnlyAllowsLockIn()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            
            Assert.That(_machine.IsActionAllowed(GameAction.LockIn), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.ConfirmSelection), Is.False);
        }

        [Test]
        public void IsActionAllowed_NightSurvival_ReturnsTrueForMovementActions()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            
            Assert.That(_machine.IsActionAllowed(GameAction.Move), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.Sprint), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.Dash), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.Swing), Is.True);
            Assert.That(_machine.IsActionAllowed(GameAction.Glide), Is.True);
        }

        [Test]
        public void IsActionAllowed_Resolve_ReturnsFalseForAll()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            _machine.TryTransition(PhaseState.Resolve);
            
            Assert.That(_machine.IsActionAllowed(GameAction.Move), Is.False);
            Assert.That(_machine.IsActionAllowed(GameAction.InspectSoul), Is.False);
        }

        [Test]
        public void TryTransition_InvalidFromResolve_Blocked()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            _machine.TryTransition(PhaseState.Resolve);
            
            var result = _machine.TryTransition(PhaseState.DayService);
            Assert.That(result, Is.False);
        }

        [Test]
        public void TryTransition_FatalError_FromChoiceLock()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            var result = _machine.TryTransition(PhaseState.FatalError);
            
            Assert.That(result, Is.True);
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.FatalError));
        }

        [Test]
        public void TryTransition_Reset_FromResolve()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            _machine.TryTransition(PhaseState.Resolve);
            
            var result = _machine.TryTransition(PhaseState.Reset);
            
            Assert.That(result, Is.True);
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.Reset));
        }

        [Test]
        public void TryTransition_CanGoBackToDayService_FromReset()
        {
            _machine.TryTransition(PhaseState.ChoiceLock);
            _machine.TryTransition(PhaseState.NightSurvival);
            _machine.TryTransition(PhaseState.Resolve);
            _machine.TryTransition(PhaseState.Reset);
            
            var result = _machine.TryTransition(PhaseState.DayService);
            
            Assert.That(result, Is.True);
            Assert.That(_machine.CurrentState, Is.EqualTo(PhaseState.DayService));
        }
    }
}