// Assets/_Project/Domain/Services/IWardTimerService.cs
using R3;

namespace SolarPhobia.Domain
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
        ReadOnlyReactiveProperty<float> WardTimer { get; }
    }
}
