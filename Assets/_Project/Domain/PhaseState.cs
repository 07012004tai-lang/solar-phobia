// Assets/_Project/Domain/PhaseState.cs
namespace SolarPhobia.Domain
{
    /// <summary>
    /// Phase states for the game's day/night cycle state machine.
    /// </summary>
    public enum PhaseState
    {
        Bootstrapping,
        DayService,
        ChoiceLock,
        NightSurvival,
        Resolve,
        Reset,
        FatalError
    }
}
