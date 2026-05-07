namespace SolarPhobia.Domain.ValueObjects
{
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