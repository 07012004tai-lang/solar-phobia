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
        /// Set the drain rate with bone and hallucination multipliers.
        /// Formula: baseDrainRate × (1 + boneCount × 0.25) × (1 + hallucinationMultiplier)
        /// </summary>
        void SetDrainRate(float baseDrainRate, int boneCount, float hallucinationMultiplier);

        /// <summary>
        /// Observable for ward timer value changes.
        /// </summary>
        ReadOnlyReactiveProperty<float> WardTimer { get; }
    }
}
