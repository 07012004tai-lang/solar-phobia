using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Messages
{
    public readonly struct PhaseChangedEvent
    {
        public readonly PhaseState PreviousPhase;
        public readonly PhaseState NewPhase;

        public PhaseChangedEvent(PhaseState previousPhase, PhaseState newPhase)
        {
            PreviousPhase = previousPhase;
            NewPhase = newPhase;
        }
    }

    public readonly struct DayStartEvent
    {
    }

    public readonly struct NightStartEvent
    {
    }

    public readonly struct ResolveEvent
    {
    }

    public readonly struct SelectionChangedEvent
    {
        public readonly string SoulId;
        public readonly DaySelectionState OldState;
        public readonly DaySelectionState NewState;

        public SelectionChangedEvent(string soulId, DaySelectionState oldState, DaySelectionState newState)
        {
            SoulId = soulId;
            OldState = oldState;
            NewState = newState;
        }
    }

    public readonly struct NightOutcomeEvent
    {
        public readonly string SoulId;
        public readonly NightOutcomeState Outcome;

        public NightOutcomeEvent(string soulId, NightOutcomeState outcome)
        {
            SoulId = soulId;
            Outcome = outcome;
        }
    }
}