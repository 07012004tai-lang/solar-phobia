// Assets/_Project/Domain/Services/ISensoryTierService.cs
using R3;
using SolarPhobia.Domain.Events;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Domain
{
    /// <summary>
    /// Interface for sensory tier feedback service.
    /// Implements TR-state-005: Sensory tiers trigger at 75%, 50%, 25%, ≤10s thresholds.
    /// </summary>
    public interface ISensoryTierService
    {
        /// <summary>
        /// Observable that emits when sensory tier changes (one-shot per crossing).
        /// </summary>
        Observable<SensoryTierChangedEvent> OnTierChanged { get; }

        /// <summary>
        /// Current sensory tier (reactive property for continuous monitoring).
        /// </summary>
        ReadOnlyReactiveProperty<SensoryTier> CurrentTier { get; }

        /// <summary>
        /// Initialize the service with Ward timer observable.
        /// </summary>
        void Initialize(ReadOnlyReactiveProperty<float> wardObservable, float maxWard);
    }
}
