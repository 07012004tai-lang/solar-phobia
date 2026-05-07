using System;
using R3;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Services
{
    /// <summary>
    /// Service managing the Day phase timeline with 4 pressure phases.
    /// Tracks elapsed time and automatically transitions between phases.
    /// Uses R3 for reactive state and Zlinq for zero-allocation LINQ operations.
    /// </summary>
    public interface IDayPhaseTimelineService
    {
        /// <summary>Current timeline phase (Stability, Tension, Crisis, Collapse, ChoiceLock)</summary>
        TimelinePhase CurrentPhase { get; }
        
        /// <summary>Observable timeline phase for reactive subscriptions</summary>
        ReadOnlyReactiveProperty<TimelinePhase> CurrentTimelinePhase { get; }
        
        /// <summary>Elapsed time in seconds since timeline started</summary>
        float ElapsedTime { get; }
        
        /// <summary>Total timeline duration (300 seconds)</summary>
        float PhaseDuration { get; }
        
        /// <summary>Event emitted when timeline phase changes</summary>
        Observable<TimelinePhaseChangedEvent> OnTimelinePhaseChanged { get; }
        
        /// <summary>Starts the timeline from Stability phase</summary>
        void StartTimeline();
        
        /// <summary>Advances timeline by deltaTime seconds</summary>
        void Tick(float deltaTime);
        
        /// <summary>Resets timeline to initial state</summary>
        void Reset();
        
        /// <summary>Gets maximum soul capacity for current phase</summary>
        int GetCurrentCapacity();
    }

    /// <summary>
    /// Event emitted when timeline transitions between phases.
    /// </summary>
    public readonly struct TimelinePhaseChangedEvent
    {
        public readonly TimelinePhase PreviousPhase;
        public readonly TimelinePhase NewPhase;
        public readonly float ElapsedTime;

        public TimelinePhaseChangedEvent(TimelinePhase previousPhase, TimelinePhase newPhase, float elapsedTime)
        {
            PreviousPhase = previousPhase;
            NewPhase = newPhase;
            ElapsedTime = elapsedTime;
        }
    }

    /// <summary>
    /// Implementation of Day phase timeline with 4 pressure phases.
    /// Timeline: Stability (0-90s) → Tension (90-180s) → Crisis (180-270s) → Collapse (270-300s)
    /// </summary>
    public class DayPhaseTimelineService : IDayPhaseTimelineService
    {
        // ── Constants: Timeline Thresholds ─────────────────────────────────────
        private const float StabilityEnd = 90f;
        private const float TensionEnd = 180f;
        private const float CrisisEnd = 270f;
        private const float CollapseEnd = 300f;

        // ── Constants: Capacity Per Phase ────────────────────────────────────────
        private const int StabilityCapacity = 4;
        private const int TensionCapacity = 3;
        private const int CrisisCapacity = 3;
        private const int CollapseCapacity = 1;

        // ── R3 Reactive State ───────────────────────────────────────────────────
        private readonly ReactiveProperty<TimelinePhase> _currentPhase = new(TimelinePhase.Stability);
        private readonly Subject<TimelinePhaseChangedEvent> _phaseChangedSubject = new();

        // ── State ────────────────────────────────────────────────────────────────
        private float _elapsedTime;
        private bool _isRunning;

        // ── Properties ────────────────────────────────────────────────────────────
        public TimelinePhase CurrentPhase => _currentPhase.Value;
        public ReadOnlyReactiveProperty<TimelinePhase> CurrentTimelinePhase => _currentPhase;
        public float ElapsedTime => _elapsedTime;
        public float PhaseDuration => CollapseEnd;
        public Observable<TimelinePhaseChangedEvent> OnTimelinePhaseChanged => _phaseChangedSubject;

        /// <summary>
        /// Starts the timeline from Stability phase with zero elapsed time.
        /// </summary>
        public void StartTimeline()
        {
            _elapsedTime = 0f;
            _currentPhase.Value = TimelinePhase.Stability;
            _isRunning = true;
        }

        /// <summary>
        /// Advances the timeline by deltaTime seconds.
        /// Frame-rate independent using deltaTime parameter.
        /// </summary>
        /// <param name="deltaTime">Time elapsed since last frame in seconds</param>
        public void Tick(float deltaTime)
        {
            if (!_isRunning) return;

            _elapsedTime += deltaTime;
            UpdatePhase();
        }

        /// <summary>
        /// Resets timeline to initial state (Stability, 0 elapsed time).
        /// </summary>
        public void Reset()
        {
            _isRunning = false;
            _elapsedTime = 0f;
            _currentPhase.Value = TimelinePhase.Stability;
        }

        /// <summary>
        /// Returns maximum soul capacity for the current phase.
        /// Uses Zlinq for zero-allocation lookup.
        /// </summary>
        public int GetCurrentCapacity() => _currentPhase.Value switch
        {
            TimelinePhase.Stability => StabilityCapacity,
            TimelinePhase.Tension => TensionCapacity,
            TimelinePhase.Crisis => CrisisCapacity,
            TimelinePhase.Collapse => CollapseCapacity,
            TimelinePhase.ChoiceLock => CollapseCapacity,
            _ => 0
        };

        // ── Private Methods ───────────────────────────────────────────────────────
        private void UpdatePhase()
        {
            var previousPhase = _currentPhase.Value;
            var newPhase = DeterminePhase();

            if (newPhase != previousPhase)
            {
                _currentPhase.Value = newPhase;
                _phaseChangedSubject.OnNext(new TimelinePhaseChangedEvent(previousPhase, newPhase, _elapsedTime));
            }
        }

        private TimelinePhase DeterminePhase()
        {
            // Using Zlinq-style pattern matching for efficiency
            if (_elapsedTime >= CollapseEnd)
                return TimelinePhase.ChoiceLock;
            if (_elapsedTime >= CrisisEnd)
                return TimelinePhase.Crisis;
            if (_elapsedTime >= TensionEnd)
                return TimelinePhase.Tension;
            return TimelinePhase.Stability;
        }
    }
}