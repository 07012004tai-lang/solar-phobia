using SolarPhobia.Domain;

namespace SolarPhobia.Application.Services
{
    public class PhaseStateMachine
    {
        public GamePhase CurrentPhase { get; private set; } = GamePhase.BOOT;

        public void TransitionTo(GamePhase phase)
        {
            CurrentPhase = phase;
        }
    }
}

