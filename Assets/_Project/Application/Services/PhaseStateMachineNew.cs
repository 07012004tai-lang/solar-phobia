using System;
using System.Collections.Generic;
using R3;
using SolarPhobia.Application.Messages;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    public interface IPhaseStateMachine
    {
        PhaseState CurrentState { get; }
        ReadOnlyReactiveProperty<PhaseState> CurrentPhase { get; }
        Observable<PhaseChangedEvent> OnPhaseChanged { get; }
        Observable<DayStartEvent> OnDayStart { get; }
        Observable<NightStartEvent> OnNightStart { get; }
        Observable<ResolveEvent> OnResolve { get; }
        bool TryTransition(PhaseState newPhase);
        bool IsActionAllowed(GameAction action);
        void Initialize();
    }

    public class PhaseStateMachine : IPhaseStateMachine
    {
        private readonly ReactiveProperty<PhaseState> _currentPhase = new(PhaseState.Bootstrapping);
        private readonly Subject<PhaseChangedEvent> _phaseChangedSubject = new();
        private readonly Subject<DayStartEvent> _dayStartSubject = new();
        private readonly Subject<NightStartEvent> _nightStartSubject = new();
        private readonly Subject<ResolveEvent> _resolveSubject = new();
        private readonly Dictionary<PhaseState, HashSet<GameAction>> _phaseContracts;

        public PhaseState CurrentState => _currentPhase.Value;
        public ReadOnlyReactiveProperty<PhaseState> CurrentPhase => _currentPhase;
        public Observable<PhaseChangedEvent> OnPhaseChanged => _phaseChangedSubject;
        public Observable<DayStartEvent> OnDayStart => _dayStartSubject;
        public Observable<NightStartEvent> OnNightStart => _nightStartSubject;
        public Observable<ResolveEvent> OnResolve => _resolveSubject;

        public PhaseStateMachine()
        {
            _phaseContracts = BuildPhaseContracts();
        }

        public void Initialize()
        {
            TransitionTo(PhaseState.DayService);
        }

        public bool TryTransition(PhaseState newPhase)
        {
            if (!IsValidTransition(_currentPhase.Value, newPhase))
            {
                return false;
            }

            TransitionTo(newPhase);
            return true;
        }

        private void TransitionTo(PhaseState newPhase)
        {
            var previousPhase = _currentPhase.Value;
            _currentPhase.Value = newPhase;

            _phaseChangedSubject.OnNext(new PhaseChangedEvent(previousPhase, newPhase));

            switch (newPhase)
            {
                case PhaseState.DayService:
                    _dayStartSubject.OnNext(new DayStartEvent());
                    break;
                case PhaseState.NightSurvival:
                    _nightStartSubject.OnNext(new NightStartEvent());
                    break;
                case PhaseState.Resolve:
                    _resolveSubject.OnNext(new ResolveEvent());
                    break;
            }
        }

        public bool IsActionAllowed(GameAction action)
        {
            return _phaseContracts.TryGetValue(_currentPhase.Value, out var allowed)
                   && allowed.Contains(action);
        }

        private bool IsValidTransition(PhaseState from, PhaseState to)
        {
            return to switch
            {
                PhaseState.DayService => from == PhaseState.Bootstrapping || from == PhaseState.Reset,
                PhaseState.ChoiceLock => from == PhaseState.DayService,
                PhaseState.NightSurvival => from == PhaseState.ChoiceLock,
                PhaseState.Resolve => from == PhaseState.NightSurvival,
                PhaseState.Reset => from == PhaseState.Resolve,
                PhaseState.FatalError => from == PhaseState.ChoiceLock || from == PhaseState.NightSurvival,
                _ => false
            };
        }

        private Dictionary<PhaseState, HashSet<GameAction>> BuildPhaseContracts()
        {
            return new Dictionary<PhaseState, HashSet<GameAction>>
            {
                [PhaseState.DayService] = new()
                {
                    GameAction.InspectSoul,
                    GameAction.AssignRitual,
                    GameAction.ConfirmSelection,
                    GameAction.CancelSelection
                },
                [PhaseState.ChoiceLock] = new()
                {
                    GameAction.LockIn
                },
                [PhaseState.NightSurvival] = new()
                {
                    GameAction.Move,
                    GameAction.Sprint,
                    GameAction.Dash,
                    GameAction.Swing,
                    GameAction.Glide,
                    GameAction.Crouch,
                    GameAction.InteractShrine
                },
                [PhaseState.Resolve] = new(),
                [PhaseState.Reset] = new()
            };
        }
    }
}