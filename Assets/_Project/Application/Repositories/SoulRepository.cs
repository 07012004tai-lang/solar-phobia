using System.Collections.Generic;
using System.Linq;
using R3;
using SolarPhobia.Application.Messages;
using SolarPhobia.Domain.ValueObjects;

namespace SolarPhobia.Application.Repositories
{
    public class Soul
    {
        public string Id { get; }
        public string LocalizedName { get; }

        public DaySelectionState DaySelection { get; internal set; }
        public NightOutcomeState NightOutcome { get; internal set; }
        public LifeState Life { get; internal set; }

        public Soul(string id, string localizedName)
        {
            Id = id;
            LocalizedName = localizedName;
            DaySelection = DaySelectionState.Unselected;
            NightOutcome = NightOutcomeState.None;
            Life = LifeState.Alive;
        }
    }

    public interface ISoulRepository
    {
        IReadOnlyList<Soul> Souls { get; }
        Soul GetSoul(string id);
        bool TrySetSelection(string soulId, DaySelectionState state, PhaseState currentPhase);
        bool TrySetNightOutcome(string soulId, NightOutcomeState outcome, PhaseState currentPhase);
        IReadOnlyList<Soul> GetSavedSouls();
        IReadOnlyList<Soul> GetAbandonedSoul();
        bool IsSelectionValid(int requiredSaved);
        void Reset();
        Observable<SelectionChangedEvent> OnSelectionChanged { get; }
    }

    public class SoulRepository : ISoulRepository
    {
        private readonly Dictionary<string, Soul> _souls;
        private readonly Subject<SelectionChangedEvent> _selectionSubject = new();

        public IReadOnlyList<Soul> Souls => _souls.Values.ToList();
        public Observable<SelectionChangedEvent> OnSelectionChanged => _selectionSubject;

        public SoulRepository()
        {
            _souls = new Dictionary<string, Soul>
            {
                ["linh"] = new Soul("linh", "Em Linh"),
                ["van"] = new Soul("van", "Ông Văn"),
                ["minh"] = new Soul("minh", "Anh Minh")
            };
        }

        public Soul GetSoul(string id)
        {
            return _souls.TryGetValue(id, out var soul) ? soul : null;
        }

        public bool TrySetSelection(string soulId, DaySelectionState state, PhaseState currentPhase)
        {
            if (currentPhase != PhaseState.DayService)
            {
                UnityEngine.Debug.LogWarning($"SoulRepository: Cannot write selection outside DayService phase (current: {currentPhase})");
                return false;
            }

            var soul = GetSoul(soulId);
            if (soul == null)
            {
                return false;
            }

            if (state == DaySelectionState.Saved && soul.DaySelection == DaySelectionState.Abandoned)
            {
                UnityEngine.Debug.LogError($"SoulRepository: Cannot mark {soulId} as Saved when Abandoned");
                return false;
            }

            var oldState = soul.DaySelection;
            soul.DaySelection = state;

            _selectionSubject.OnNext(new SelectionChangedEvent(soulId, oldState, state));

            return true;
        }

        public bool TrySetNightOutcome(string soulId, NightOutcomeState outcome, PhaseState currentPhase)
        {
            if (currentPhase != PhaseState.ChoiceLock && currentPhase != PhaseState.NightSurvival)
            {
                UnityEngine.Debug.LogWarning($"SoulRepository: Cannot write night outcome outside ChoiceLock/NightSurvival");
                return false;
            }

            var soul = GetSoul(soulId);
            if (soul == null || soul.DaySelection != DaySelectionState.Abandoned)
            {
                UnityEngine.Debug.LogError($"SoulRepository: Night outcome only valid for abandoned soul");
                return false;
            }

            soul.NightOutcome = outcome;
            return true;
        }

        public IReadOnlyList<Soul> GetSavedSouls()
        {
            return _souls.Values.Where(s => s.DaySelection == DaySelectionState.Saved).ToList();
        }

        public IReadOnlyList<Soul> GetAbandonedSoul()
        {
            return _souls.Values.Where(s => s.DaySelection == DaySelectionState.Abandoned).ToList();
        }

        public bool IsSelectionValid(int requiredSaved)
        {
            int savedCount = GetSavedSouls().Count;
            int abandonedCount = GetAbandonedSoul().Count;

            return savedCount == requiredSaved &&
                   abandonedCount == _souls.Count - requiredSaved;
        }

        public void Reset()
        {
            foreach (var soul in _souls.Values)
            {
                soul.DaySelection = DaySelectionState.Unselected;
                soul.NightOutcome = NightOutcomeState.None;
                soul.Life = LifeState.Alive;
            }
        }
    }
}