// Assets/_Project/Domain/Services/IVisualEffectsService.cs
namespace SolarPhobia.Domain.Services
{
    /// <summary>
    /// Interface for visual effects service.
    /// </summary>
    public interface IVisualEffectsService
    {
        /// <summary>
        /// Trigger a screen shake effect.
        /// </summary>
        void TriggerScreenShake();

        /// <summary>
        /// Trigger a red flash effect.
        /// </summary>
        void TriggerRedFlash();
    }
}
