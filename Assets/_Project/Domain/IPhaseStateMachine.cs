// Assets/_Project/Domain/IPhaseStateMachine.cs
using R3;

namespace SolarPhobia.Domain
{
    /// <summary>
    /// Interface for the phase state machine.
    /// </summary>
    public interface IPhaseStateMachine
    {
        /// <summary>
        /// Observable current phase state.
        /// </summary>
        ReadOnlyReactiveProperty<PhaseState> CurrentPhase { get; }

        /// <summary>
        /// Try to transition to a new phase.
        /// </summary>
        bool TryTransition(PhaseState newPhase);

        /// <summary>
        /// Check if a game action is allowed in the current phase.
        /// </summary>
        bool IsActionAllowed(GameAction action);
    }

    /// <summary>
    /// Game actions that can be gated by phase.
    /// </summary>
    public enum GameAction
    {
        InspectSoul,
        AssignRitual,
        ConfirmSelection,
        CancelSelection,
        LockIn,
        Move,
        Sprint,
        Dash,
        Swing,
        Glide,
        Crouch,
        InteractShrine
    }
}
