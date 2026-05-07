// Assets/_Project/Domain/Services/IWardTimerService.cs
namespace SolarPhobia.Domain.Services
{
    /// <summary>
    /// Interface for ward timer service.
    /// </summary>
    public interface IWardTimerService
    {
        /// <summary>
        /// Apply a penalty to the ward timer.
        /// </summary>
        void ApplyPenalty(float amount);

        /// <summary>
        /// Observable for ward timer value changes.
        /// </summary>
        R3.ReadOnlyReactiveProperty<float> WardTimer { get; }
    }
}
