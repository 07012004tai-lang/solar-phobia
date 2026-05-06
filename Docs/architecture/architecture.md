# Solar Phobia: Nắng Gắt — Master Architecture

## Document Status
- Version: 1
- Last Updated: 2026-05-06
- Engine: Unity 6.3 LTS (Unity 6000.3.11f1)
- GDDs Covered: game-concept.md, systems-index.md, game-state-phase-state-machine.md, health-stamina-damage-rules.md, map-and-spawn-director.md, npc-soul-data-model.md
- ADRs Referenced: None yet (to be populated in Phase 5)

## Engine Knowledge Gap Summary

**Engine**: Unity 6.3 LTS (released Dec 2025)
**LLM Training Covers**: up to ~Unity 2022 LTS
**Post-Cutoff Versions**: 6.0 (HIGH), 6.1 (MEDIUM), 6.2 (MEDIUM), 6.3 LTS (HIGH)

### HIGH RISK Domains (must verify against engine reference)
- **DOTS/ECS**: Complete API overhaul in Entities 1.3+. Old `ComponentSystem` → new `ISystem` + `IJobEntity`. Project uses C# 9.0 with .NET 4.7.1 — Entities package compatibility TBD.
- **Input System**: Legacy `Input` class deprecated. Must use `com.unity.inputsystem` package. All input code must use `Keyboard.current`, `Mouse.current`, `InputAction` callbacks.
- **Rendering**: URP/HDRP upgrades, RenderGraph API replaces old `CommandBuffer.Execute()` pattern. Project uses URP per AGENTS.md.

### MEDIUM RISK Domains (verify key APIs)
- **Addressables**: Asset loading now throws exceptions by default (was silent null). Need try/catch or `TryLoad` variants.
- **Physics**: Default solver iterations changed. `Physics.defaultSolverIterations` may differ from training data.
- **UI**: UI Toolkit is production-ready and recommended over UGUI for new projects.

### LOW RISK Domains (in training data, likely reliable)
- Basic MonoBehaviour lifecycle: `Awake()`, `Start()`, `Update()` unchanged
- GameObject/Transform: Core scene graph unchanged
- Debug.Log: Still valid for diagnostics
- Basic C# types: `List<T>`, `Dictionary<K,V>`, `IEnumerable<T>` unchanged

### Systems from GDD that touch HIGH/MEDIUM risk domains:
- **Map & Spawn Director** → Core/Map → **MEDIUM risk** (physics overlap checks, collider queries need `Physics.RaycastNonAlloc` instead of deprecated `RaycastAll`)
- **Player Controller** (not yet designed) → Core → **HIGH risk** (input handling must use new Input System)
- **Boss Chase AI** (not yet designed) → Feature → **MEDIUM risk** (pathfinding, physics)
- **HUD & Diegetic Feedback** (not yet designed) → Presentation → **MEDIUM risk** (should use UI Toolkit, not UGUI)
- **Curse Effect Modules** (not yet designed) → Feature → **MEDIUM risk** (may use VFX Graph, Shader programming)

---

## System Layer Map

```
┌─────────────────────────────────────────────┐
│  PRESENTATION LAYER                         │  ← HUD & Diegetic Feedback, Audio State Director
├─────────────────────────────────────────────┤
│  FEATURE LAYER                              │  ← Day Service & Selection, Consequence Resolver,
│                                         │    Curse Effect Modules, Night Survival Run,
│                                         │    Boss Chase AI
├─────────────────────────────────────────────┤
│  CORE LAYER                                 │  ← Player Controller, Shrine Objective & Win/Lose,
│                                         │    Resource Effects, Solar Residue Hazard,
│                                         │    Health/Stamina & Damage Rules,
│                                         │    Map & Spawn Director
├─────────────────────────────────────────────┤
│  FOUNDATION LAYER                           │  ← Game State / Phase State Machine,
│                                         │    NPC/Soul Data Model, Save Seed / Run Reset
├─────────────────────────────────────────────┤
│  PLATFORM LAYER                             │  ← Unity 6.3 LTS API surface, OS
└─────────────────────────────────────────────┘
```

### Layer Assignments

| # | System | Layer | Engine Risk | Notes |
|---|--------|-------|-------------|-------|
| 1 | Game State / Phase State Machine | Foundation | LOW | Pure C# state machine, no engine API |
| 2 | NPC/Soul Data Model | Foundation | LOW | Data structures, no engine API |
| 3 | Map & Spawn Director | Core | MEDIUM | Physics queries, collider checks — use `Physics.RaycastNonAlloc` not `RaycastAll` |
| 4 | Health/Stamina & Damage Rules | Core | LOW | Timers, math, pure logic |
| 5 | Player Controller | Core | HIGH | Must use new Input System (`com.unity.inputsystem`), not legacy `Input` class |
| 6 | Shrine Objective & Win/Lose Rules | Core | LOW | State evaluation |
| 7 | Resource Effects | Feature | LOW | Data transformation |
| 8 | Solar Residue Hazard | Core | MEDIUM | Hazard zones, physics — use `RenderGraph API` for custom passes |
| 9 | Day Service & Selection | Feature | LOW | UI interaction (use UI Toolkit, not UGUI) |
| 10 | Consequence Resolver | Feature | LOW | Data transformation |
| 11 | Curse Effect Modules | Feature | MEDIUM | May use VFX Graph (not Legacy Particle System), custom shaders |
| 12 | Boss Chase AI | Feature | MEDIUM | Pathfinding, AI behavior — use new NavMesh or custom |
| 13 | Night Survival Run | Feature | LOW | Orchestration |
| 14 | HUD & Diegetic Feedback | Presentation | MEDIUM | Must use UI Toolkit (`UIDocument`, `VisualElement`), not UGUI Canvas |
| 15 | Audio State Director | Presentation | LOW | AudioMixer unchanged |
| 16 | Save Seed / Run Reset | Foundation | LOW | PlayerPrefs or file I/O |

---

## Module Ownership

### Foundation Layer

#### Game State / Phase State Machine
- **Owns**: Phase state enum (`Bootstrapping`, `DayService`, `ChoiceLock`, `NightSurvival`, `Resolve`, `Reset`, `FatalError`), phase transition logic, phase event dispatch, auto-commit policy, FatalError retry count
- **Exposes**: `OnPhaseChanged` event, `CurrentPhase` property, `IsPhaseActive(Phase)` query, `RegisterPhaseListener()` / `UnregisterPhaseListener()`
- **Consumes**: Day Service selection payload (validated selection + consequence payload), Map & Spawn spawn bundle validation, Shrine Objective win/lose signals
- **Engine APIs used**: None (pure C#) — ⚠️ LOW risk, no version concerns

#### NPC/Soul Data Model
- **Owns**: Soul records (`SoulId`, `DaySelectionState`, `NightOutcomeState`, `LifeState`, `RecordLockState`), selection validity logic, priority scoring, data health ratio
- **Exposes**: `GetSoul(SoulId)`, `OnSelectionChanged` event, `OnNightOutcomeAssigned` event, `IsSelectionValid` query
- **Consumes**: None (data provider only, written to by authorized systems via phase-locked contracts)
- **Engine APIs used**: None (pure C# data structures) — ⚠️ LOW risk, no version concerns

#### Save Seed / Run Reset
- **Owns**: Run seed value, reset logic for all run-scoped state, snapshot serialization
- **Exposes**: `ResetAll()` method, `OnModelReset` event, `GetRunSnapshot()` method
- **Consumes**: Game State phase signals (Reset phase), NPC Model snapshot, Map seed data
- **Engine APIs used**: `PlayerPrefs` (fallback) or `System.IO` for file serialization — ⚠️ LOW risk (stable APIs)

---

### Core Layer

#### Player Controller
- **Owns**: Player input state, movement state (grounded, jumping, dashing, gliding), ward threshold penalties (dash cooldown, recovery delay)
- **Exposes**: `OnPlayerDeath` event, `CurrentWardPercent` query, `IsInCover` query
- **Consumes**: Game State phase mode (input enabled/disabled by phase), Health/Stamina ward thresholds (for movement penalties), Map cover zone signals, Boss Chase AI threat proximity
- **Engine APIs used**: ⚠️ **HIGH RISK** — `InputSystem` package (`Keyboard`, `Mouse`, `Gamepad`), `CharacterController` or `Rigidbody`, `Transform` — Verify against Unity 6.3 Input System docs

#### Health/Stamina & Damage Rules
- **Owns**: Ward Timer value, ward percent tiers (75%, 50%, 25%, ≤10s), penalty application logic, safe zone state, hallucination multiplier
- **Exposes**: `OnWardPenaltyApplied` event, `OnWardTierChanged` event, `OnWardDepleted` event, `OnWardSafeZoneChanged` event, `CurrentWardSec` query
- **Consumes**: Game State phase activation (NightSurvival only), Map strike/hazard/relic drain events, Boss Chase AI strike events, Shrine safe zone signals
- **Engine APIs used**: None (pure C# math + events) — ⚠️ LOW risk

#### Map & Spawn Director
- **Owns**: Lane geometry, fog settings, grave mound placement (SafeMound, CursedMound, FalseSafeMound), boss searchlight sweep pattern, strike telegraph timing, route viability data, cover density
- **Exposes**: `OnStrikeHit` event, `OnRelicPickedUp` event, `OnCoverEnter/Exit` events, `IsRouteViable` query, `GetSpawnBundle()` method
- **Consumes**: Game State phase lifecycle (NightSurvival start/end), NPC abandoned soul data (for consequence-biased spawn), Health ward viability constraints
- **Engine APIs used**: ⚠️ **MEDIUM RISK** — `Collider` overlap checks (use `Physics.OverlapSphereNonAlloc` not `OverlapSphere`), `GameObject` spawn/destroy, `Fog` settings, `RenderGraph` API for custom passes — Verify against Unity 6.3 URP docs

#### Shrine Objective & Win/Lose Rules
- **Owns**: Shrine trigger volume, win/lose condition evaluation, outcome snapshot
- **Exposes**: `OnShrineReached` event, `OnPlayerDeath` event (consume from Health), `GetOutcomeSnapshot()` method
- **Consumes**: Game State phase transitions, Health WardTimer ≤0 signal, Player Controller shrine trigger collision
- **Engine APIs used**: `Collider` trigger events — ⚠️ LOW risk (stable API)

#### Resource Effects
- **Owns**: Resource definitions (tea = light, incense = safe zone, offering = skill), effect application logic
- **Exposes**: `OnResourceConsumed` event, `GetAvailableResources` query
- **Consumes**: NPC saved soul set (determines available resources), Game State phase (DayService only)
- **Engine APIs used**: None (data transformation) — ⚠️ LOW risk

#### Solar Residue Hazard
- **Owns**: Hazard zone definitions, Linh's Cursed Tide (DoT), Van's Blood Net (flat hit), Mirage fall zones
- **Exposes**: `OnHazardTriggered` event, `GetHazardPenalty(penaltyType)` query
- **Consumes**: Map hazard zone contacts, Game State phase (NightSurvival only), Health penalty caps
- **Engine APIs used**: `Collider` trigger events — ⚠️ MEDIUM risk (use non-alloc physics)

---

### Feature Layer

#### Day Service & Selection
- **Owns**: Soul selection UI state, selection validation display, confirmation flow
- **Exposes**: `OnSelectionConfirmed` event (payload with saved/abandoned souls), `GetSelectionValidity()` query
- **Consumes**: NPC Soul Model (read/write selection state), Game State phase (DayService only), Resource Effects available resources
- **Engine APIs used**: ⚠️ MEDIUM risk — UI Toolkit (`UIDocument`, `VisualElement`, `Button`, `Label`) — NOT UGUI Canvas

#### Consequence Resolver
- **Owns**: Curse mapping logic (abandoned soul → Drag/Block/FakeShrine), curse payload assembly
- **Exposes**: `OnCursePayloadReady` event, `GetCursePayload()` method
- **Consumes**: NPC abandoned soul identity + NightOutcomeState, Game State ChoiceLock phase trigger
- **Engine APIs used**: None (pure C# logic) — ⚠️ LOW risk

#### Curse Effect Modules
- **Owns**: Curse behavior implementations (Drag = boss speed boost, Block = path obstruction, FakeShrine = false endpoint)
- **Exposes**: `OnCurseActivated` event, `GetCurseEffects()` query
- **Consumes**: Consequence Resolver curse payload, Player Controller position, Map lane geometry
- **Engine APIs used**: ⚠️ MEDIUM RISK — VFX Graph (not Legacy Particle System), custom shaders — Verify against Unity 6.3 VFX docs

#### Boss Chase AI
- **Owns**: Boss searchlight behavior, chase intensity, strike timing, patrol pattern
- **Exposes**: `OnStrikeTelegraph` event, `OnStrikeResolved` event, `GetBossThreatLevel()` query
- **Consumes**: Game State NightSurvival phase, Map sweep pattern data, Player Controller position
- **Engine APIs used**: ⚠️ MEDIUM RISK — Pathfinding (NavMesh or custom), `Transform` tracking — Verify against Unity 6.3 AI docs

#### Night Survival Run
- **Owns**: Night loop orchestration, objective tracking (reach shrine), survival pressure coordination
- **Exposes**: `OnNightComplete` event (success/failure), `GetNightStatus()` query
- **Consumes**: Game State NightSurvival phase, Shrine Objective outcome, Health ward depletion, Boss Chase AI threat, Solar Hazard active effects, Resource Effects consumables
- **Engine APIs used**: None (orchestration logic) — ⚠️ LOW risk

---

### Presentation Layer

#### HUD & Diegetic Feedback
- **Owns**: Ward timer display, ward tier styling, penalty reason ribbon, action cost preview, safe zone status, strike warning icons, false cover warning
- **Exposes**: None (consumes events, renders UI)
- **Consumes**: Health WardTimer + tier events, Map strike/curse/relic events, Day Service selection UI, Night Survival objective status
- **Engine APIs used**: ⚠️ **MEDIUM RISK** — UI Toolkit (`UIDocument`, `VisualElement`, `ProgressBar`, `Label`, `Button`) — NOT UGUI Canvas. Use UXML (structure) + USS (styling).

#### Audio State Director
- **Owns**: Day/night mix snapshots, threat escalation layers (wind, wave, whisper, heartbeat), phase transition stingers
- **Exposes**: None (consumes events, controls AudioMixer)
- **Consumes**: Game State phase changes, Health ward tier changes, Boss Chase AI threat level, Night Survival outcome
- **Engine APIs used**: `AudioMixer`, `AudioSource` — ⚠️ LOW risk (stable APIs)

---

### Dependency Diagram (ASCII)

```
[Platform Layer: Unity 6.3 LTS API]
        ↑
[Foundation Layer]
| Game State / Phase State Machine ←── NPC/Soul Data Model
|        ↑                           |
|   Save Seed / Run Reset ←──────────┘
        ↑
[Core Layer]
| Player Controller ←── Health/Stamina & Damage Rules
|        ↑                    ↑
| Shrine Objective    ←── Map & Spawn Director
|        ↑                    ↑
| Resource Effects ←───────┘
| Solar Residue Hazard ←── Map & Spawn Director
        ↑
[Feature Layer]
| Day Service & Selection ←── NPC/Soul Data Model, Resource Effects
|        ↑
| Consequence Resolver ←── Day Service, NPC/Soul
|        ↑
| Curse Effect Modules ←── Consequence Resolver, Player Controller, Map
|        ↑
| Boss Chase AI ←── Player Controller, Map, Health
|        ↑
| Night Survival Run ←── Shrine, Solar Hazard, Boss, Curse, Resource
        ↑
[Presentation Layer]
| HUD & Diegetic Feedback ←── Health, Map, Day Service, Night Survival
| Audio State Director ←── Game State, Boss, Night Survival, Health
```

---

## Data Flow

### 1. Frame Update Path (Night Survival)

```
Input (Keyboard/Mouse/Gamepad)
    ↓ [Player Controller: reads input via Input System package]
Player Position + Movement State
    ↓ [Map & Spawn Director: checks cover zones, hazard overlaps]
    ↓ [Boss Chase AI: evaluates threat, triggers strike if exposed]
    ↓ [Health/Stamina: applies penalties, updates Ward Timer]
    ↓ [HUD: reads Ward Timer + tier → updates UI]
Rendering (URP + RenderGraph API)
```

### 2. Event/Signal Path (Loose Coupling via C# Events)

```
Game State / Phase State Machine
    ├──→ OnPhaseChanged (Phase oldPhase, Phase newPhase)
    │       ├──→ Player Controller (enable/disable input)
    │       ├──→ Health/Stamina (enable/disable damage processing)
    │       ├──→ Map & Spawn (start/stop night map)
    │       ├──→ HUD (switch UI mode)
    │       └──→ Audio (transition mix snapshot)
    │
    └──→ OnNightStart / OnNightEnd
            ├──→ Boss Chase AI (start/stop chase)
            └──→ Night Survival Run (start/stop loop)

Health/Stamina & Damage Rules
    ├──→ OnWardPenaltyApplied(source, penalty, remaining)
    │       └──→ HUD (show penalty popup)
    ├──→ OnWardTierChanged(old, new)
    │       ├──→ Player Controller (apply movement penalties)
    │       ├──→ HUD (update tier styling)
    │       └──→ Audio (activate whisper/heartbeat layers)
    └──→ OnWardDepleted(reason)
            └──→ Shrine Objective (trigger failure)

Map & Spawn Director
    ├──→ OnStrikeHit(penalty)
    │       └──→ Health (apply ward penalty)
    ├──→ OnRelicPickedUp(relicId)
    │       └──→ Health (apply hallucination multiplier)
    └──→ OnCoverEnter/Exit(isInCover)
            └──→ Player Controller (update cover state)
```

### 3. Save/Load Path

```
Run Start (Game State: Bootstrapping)
    ↓
Save Seed / Run Reset: generate or load seed
    ↓
Map & Spawn Director: deterministic map from seed
    ↓
NPC/Soul Data Model: initialize 3 souls (Linh, Van, Minh)
    ↓
[DayService begins]
    ↓
Day Service & Selection: write selection → NPC Model
    ↓
Game State: ChoiceLock → Consequence Resolver: write NightOutcomeState → NPC Model
    ↓
[NightSurvival begins]
    ↓
[Gameplay runs...]
    ↓
Shrine Objective: success/failure → Game State: Resolve
    ↓
Save Seed / Run Reset: snapshot outcome → PlayerPrefs or file
    ↓
Reset: clear all run-scoped state → back to DayService
```

### 4. Initialization Order

1. **Platform Layer**: Unity engine boots, loads project settings
2. **Foundation Layer**:
   a. `Save Seed / Run Reset` → load or generate seed
   b. `NPC/Soul Data Model` → initialize 3 soul records
   c. `Game State / Phase State Machine` → init state machine, enter `Bootstrapping`
3. **Core Layer**:
   a. `Map & Spawn Director` → build night lane from seed
   b. `Health/Stamina & Damage Rules` → init ward config
   c. `Player Controller` → init input, register phase listener
   d. `Shrine Objective` → init shrine trigger
   e. `Resource Effects` → init resource definitions
   f. `Solar Residue Hazard` → init hazard zones
4. **Feature Layer**:
   a. `Day Service & Selection` → init selection UI (UI Toolkit)
   b. `Consequence Resolver` → register ChoiceLock listener
   c. `Curse Effect Modules` → register curse payload listener
   d. `Boss Chase AI` → init searchlight pattern
   e. `Night Survival Run` → register night lifecycle listeners
5. **Presentation Layer**:
   a. `HUD & Diegetic Feedback` → load UXML/USS, register event listeners
   b. `Audio State Director` → load mix snapshots, register phase listeners
6. **Game State**: transition from `Bootstrapping` → `DayService`

### Data Flow Flags

| Data Flow | Producer | Consumer | Type | Thread |
|-----------|----------|----------|------|--------|
| Phase change | Game State | All systems | Event (synchronous) | Main thread |
| Ward penalty | Map/Boss/Health | Health → HUD/Audio | Event (synchronous) | Main thread |
| Strike hit | Map & Spawn | Health | Event (synchronous) | Main thread |
| Selection confirm | Day Service | Game State, Consequence | Event (synchronous) | Main thread |
| Shrine reached | Player Controller | Shrine Objective | Trigger collision | Main thread |
| All flows | — | — | — | **No cross-thread data flows in MVP** |

---

## API Boundaries

### Foundation Layer Contracts

#### Game State / Phase State Machine
```csharp
// Public API (consumed by all systems)
public class GameStateController : MonoBehaviour
{
    public Phase CurrentPhase { get; }
    bool IsPhaseActive(Phase phase);
    
    // Events
    public event Action<Phase, Phase> OnPhaseChanged; // oldPhase, newPhase
    public event Action OnNightStart;
    public event Action OnNightEnd;
    public event Action OnFatalError;
    
    // Phase-specific queries
    public bool TryCommitSelection(SelectionPayload payload, out string rejectReason);
    public bool IsNightStartReady(out string reason);
}

// Selection payload (passed to Game State)
public struct SelectionPayload
{
    public SoulId[] SavedSouls;      // Exactly 2
    public SoulId AbandonedSoul;      // Exactly 1
    public bool IsValid => SavedSouls.Length == 2 && AbandonedSoul != null;
}
```

#### NPC/Soul Data Model
```csharp
// Public API (consumed by Day Service, Consequence Resolver, Resource Effects)
public class SoulDataModel : MonoBehaviour
{
    public SoulRecord GetSoul(SoulId id);
    public SoulRecord[] GetAllSouls();
    public bool IsSelectionValid(out string reason);
    
    // Events
    public event Action<SoulId, DaySelectionState> OnSelectionChanged;
    public event Action<SoulId, NightOutcomeState> OnNightOutcomeAssigned;
    public event Action OnModelReset;
    
    // Write access (phase-locked, called by authorized systems only)
    internal void SetDaySelection(SoulId id, DaySelectionState state);
    internal void SetNightOutcome(SoulId id, NightOutcomeState outcome);
    internal void ResetForNewRun();
}

public enum SoulId { Linh = 1, Van = 2, Minh = 3 }
public enum DaySelectionState { Unselected, Saved, Abandoned }
public enum NightOutcomeState { None, Drag, Block, FakeShrine }
public enum LifeState { Alive, Lost }
public enum RecordLockState { Writable, Locked }

public struct SoulRecord
{
    public SoulId Id;
    public DaySelectionState DaySelectionState;
    public NightOutcomeState NightOutcomeState;
    public LifeState LifeState;
    public RecordLockState RecordLockState;
}
```

---

### Core Layer Contracts

#### Player Controller
```csharp
// Public API (consumed by Game State, Health, HUD, Boss AI)
public class PlayerController : MonoBehaviour
{
    public float CurrentWardPercent { get; }
    public bool IsInCover { get; }
    public Vector3 Position { get; }
    
    // Events
    public event Action OnPlayerDeath;
    public event Action<Vector3> OnShrineReached;
    
    // Phase control (called by Game State)
    public void SetInputEnabled(bool enabled);
    public void ApplyWardTierPenalty(WardTier tier); // dash cooldown, etc.
}

public enum WardTier { Stable, CreepingDread, HeavyBurden, Panic, DeathSpiral }
```

#### Health/Stamina & Damage Rules
```csharp
// Public API (consumed by Player Controller, HUD, Audio, Shrine, Map, Boss)
public class WardController : MonoBehaviour
{
    public float CurrentWardSec { get; }
    public float WardPercent { get; } // (current / max_initial) * 100
    public WardTier CurrentTier { get; }
    public bool IsInSafeZone { get; }
    
    // Events
    public event Action<string, float, float> OnWardPenaltyApplied; // source, penalty, remaining
    public event Action<WardTier, WardTier> OnWardTierChanged; // old, new
    public event Action<string> OnWardDepleted; // reason
    public event Action<bool> OnWardSafeZoneChanged; // isSafe
    
    // Penalty application (called by Map, Boss, etc.)
    public void ApplyPenalty(float penaltySec, string source);
    public void SetSafeZone(bool isInSafeZone);
    
    // Phase control (called by Game State)
    public void InitializeForNight(float initialWardSec);
    public void StopProcessing();
}
```

#### Map & Spawn Director
```csharp
// Public API (consumed by Game State, Health, Player Controller, Boss AI, Night Survival)
public class MapDirector : MonoBehaviour
{
    public bool IsRouteViable(float wardRemaining, float distanceToGoal, float moveSpeed);
    public SpawnBundle GetSpawnBundle();
    
    // Events
    public event Action<float> OnStrikeHit; // penaltySec
    public event Action<string> OnRelicPickedUp; // relicId
    public event Action<bool> OnCoverEnterExit; // isInCover (player)
    public event Action OnShrineReached;
    
    // Phase control (called by Game State)
    public void InitializeForNight(int seed, SoulId abandonedSoul);
    public void StopNight();
}

public struct SpawnBundle
{
    public bool IsValid;
    public Vector3 StartShrine;
    public Vector3 EndShrine;
    public GraveMoundData[] Mounds;
    public float LaneLength;
}
```

---

### Feature Layer Contracts

#### Day Service & Selection
```csharp
// Public API (consumed by Game State, Consequence Resolver)
public class DayServiceUI : MonoBehaviour // Uses UI Toolkit, not UGUI
{
    public bool IsSelectionConfirmed { get; }
    public SelectionPayload GetConfirmedSelection();
    
    // Events
    public event Action<SelectionPayload> OnSelectionConfirmed;
    
    // Phase control (called by Game State)
    public void EnableDayUI();
    public void DisableDayUI();
    public void LockSelection(); // Entering ChoiceLock
}
```

#### Consequence Resolver
```csharp
// Public API (consumed by Game State, Curse Effect Modules)
public class ConsequenceResolver : MonoBehaviour
{
    public CursePayload GetCursePayload();
    
    // Events
    public event Action<CursePayload> OnCursePayloadReady;
    
    // Phase control (called by Game State during ChoiceLock)
    public void ResolveConsequences(SoulId abandonedSoul);
}

public struct CursePayload
{
    public SoulId AbandonedSoul;
    public CurseType CurseType; // Drag, Block, FakeShrine
    public float IntensityMultiplier;
}
```

---

### Presentation Layer Contracts

#### HUD & Diegetic Feedback
```csharp
// Uses UI Toolkit (UIDocument, VisualElement) — NOT UGUI Canvas
// Consumes events from Health, Map, Day Service, Night Survival
// No public API — pure consumer, renders UI only
```

#### Audio State Director
```csharp
// Consumes events from Game State, Health, Boss, Night Survival
// Controls AudioMixer snapshots
// No public API — pure consumer
```

---

## ADR Audit

No ADRs exist yet (docs/architecture/ only contains tr-registry.yaml).

### Traceability Coverage Check

| Req ID | Requirement | ADR Coverage | Status |
|--------|-------------|--------------|--------|
| TR-state-001 | Phase state machine with 6 states | — | ❌ GAP |
| TR-state-002 | Phase transition validation | — | ❌ GAP |
| TR-state-003 | Out-of-phase action rejection | — | ❌ GAP |
| TR-state-004 | Deterministic auto-commit policy | — | ❌ GAP |
| TR-state-005 | FatalError handling | — | ❌ GAP |
| TR-state-006 | Phase duration budgeting | — | ❌ GAP |
| TR-state-007 | Night start eligibility check | — | ❌ GAP |
| TR-state-008 | Run outcome score calculation | — | ❌ GAP |
| TR-state-009 | Phase event dispatch | — | ❌ GAP |
| TR-state-010 | Performance: phase transition < 0.5ms | — | ❌ GAP |
| TR-health-001 | Single Ward Timer (no HP/stamina) | — | ❌ GAP |
| TR-health-002 | Ward init from config + modifiers | — | ❌ GAP |
| TR-health-003 | Action costs (Swing=2, Dash=5, Glide=1/s) | — | ❌ GAP |
| TR-health-004 | Passive damage penalties | — | ❌ GAP |
| TR-health-005 | Multi-source penalty cap | — | ❌ GAP |
| TR-health-006 | Hallucination/relic multiplier | — | ❌ GAP |
| TR-health-007 | Readability thresholds (75/50/25/≤10s) | — | ❌ GAP |
| TR-health-008 | Safe zone blocks penalties | — | ❌ GAP |
| TR-health-009 | Performance: survivability update < 0.2ms/frame | — | ❌ GAP |
| TR-map-001 | Single horizontal lane | — | ❌ GAP |
| TR-map-002 | Global fog with player-centered reveal | — | ❌ GAP |
| TR-map-003 | Grave mound placement (3 types) | — | ❌ GAP |
| TR-map-004 | Boss searchlight sweep pattern | — | ❌ GAP |
| TR-map-005 | Strike telegraph and penalty | — | ❌ GAP |
| TR-map-006 | Cover validity (fully inside collider) | — | ❌ GAP |
| TR-map-007 | Bone Relic pickup → hallucination + drain | — | ❌ GAP |
| TR-map-008 | Deterministic map reset per seed | — | ❌ GAP |
| TR-map-009 | Route viability check | — | ❌ GAP |
| TR-map-010 | Cover density validation | — | ❌ GAP |
| TR-npc-001 | Three canonical souls (Linh, Van, Minh) | — | ❌ GAP |
| TR-npc-002 | DaySelectionState (Unselected/Saved/Abandoned) | — | ❌ GAP |
| TR-npc-003 | NightOutcomeState (None/Drag/Block/FakeShrine) | — | ❌ GAP |
| TR-npc-004 | LifeState (Alive/Lost) | — | ❌ GAP |
| TR-npc-005 | RecordLockState controlled by phase | — | ❌ GAP |
| TR-npc-006 | Selection validity check | — | ❌ GAP |
| TR-npc-007 | Deterministic auto-complete priority | — | ❌ GAP |
| TR-npc-008 | Data health ratio metric | — | ❌ GAP |
| TR-npc-009 | Performance: per-record lookup < 0.05ms | — | ❌ GAP |

**Count: 0 covered, 34 gaps.**

---

## Required ADRs

### Foundation Layer (must create before any coding)

1. `/architecture-decision "Phase State Machine Architecture"` → covers: TR-state-001 through TR-state-010
2. `/architecture-decision "NPC/Soul Data Model and Phase-Locked Writes"` → covers: TR-npc-001 through TR-npc-009
3. `/architecture-decision "Save Seed and Run Reset Strategy"` → covers: TR-map-008 (deterministic seed), save/load path

### Core Layer

4. `/architecture-decision "Ward Timer System - Single Lethal Resource"` → covers: TR-health-001 through TR-health-009
5. `/architecture-decision "Map & Spawn Director - Lane Generation and Hazard Placement"` → covers: TR-map-001 through TR-map-010
6. `/architecture-decision "Player Controller - Input System and Movement"` → covers: Player Controller (HIGH risk - Input System)
7. `/architecture-decision "Shrine Objective and Win/Lose Conditions"` → covers: Shrine Objective system
8. `/architecture-decision "Resource Effects System"` → covers: Resource Effects system
9. `/architecture-decision "Solar Residue Hazard System"` → covers: Solar Residue Hazard system

### Feature Layer

10. `/architecture-decision "Day Service & Selection UI - UI Toolkit Implementation"` → covers: Day Service system (MEDIUM risk - UI Toolkit)
11. `/architecture-decision "Consequence Resolver - Curse Mapping Logic"` → covers: Consequence Resolver system
12. `/architecture-decision "Curse Effect Modules - VFX and Shader Implementation"` → covers: Curse Effect Modules (MEDIUM risk - VFX Graph)
13. `/architecture-decision "Boss Chase AI - Searchlight Pattern and Threat"` → covers: Boss Chase AI (MEDIUM risk)
14. `/architecture-decision "Night Survival Run - Loop Orchestration"` → covers: Night Survival Run system

### Presentation Layer

15. `/architecture-decision "HUD & Diegetic Feedback - UI Toolkit Architecture"` → covers: HUD system (MEDIUM risk - UI Toolkit)
16. `/architecture-decision "Audio State Director - Mix Snapshots and Layering"` → covers: Audio State Director system

---

## Architecture Principles

1. **Consequence-Driven Design**: Every system must reinforce the core loop — day choices have immediate, readable night consequences. No system operates in isolation; all connect to the day/night choice→survival pipeline.

2. **Phase-Gated Processing**: No system may process data outside its authorized phase. The Game State / Phase State Machine is the single authority for phase transitions. Out-of-phase operations are rejected with explicit reason codes.

3. **Single Lethal Resource (Ward Timer)**: Night survivability uses one timer, not multiple pools (no separate HP + stamina). This keeps UI simple and consequences legible — mistakes have immediate, understandable costs.

4. **Engine Version Safety**: All engine API usage must reference Unity 6.3 LTS docs (not training data). HIGH/MEDIUM risk domains (Input System, UI Toolkit, DOTS/ECS, VFX Graph) require explicit verification against `docs/engine-reference/unity/`.

5. **Loose Coupling via Events**: Systems communicate through C# events (not direct references where possible). The Game State machine is the event hub for phase changes; individual systems emit domain events (WardPenaltyApplied, StrikeHit, etc.) for decoupled consumers.

---

## Open Questions

| Question | Owner | Deadline | Resolution |
|----------|-------|----------|------------|
| Should Player Controller use CharacterController or Rigidbody? | Lead Programmer | Before Player Controller ADR | Open |
| Should Map & Spawn use GameObject instantiation or DOTS/ECS? | Systems Designer | Before Map ADR | Open — Note: ECS is HIGH risk (major API change in Unity 6) |
| Should HUD use UXML+USS files (recommended) or pure C# UI Toolkit? | UI Programmer | Before HUD ADR | Open |
| Should Save/Reset use PlayerPrefs (simple) or JSON file (robust)? | Systems Designer | Before Save ADR | Open |
| Should Boss Chase AI use Unity NavMesh or custom steering? | AI Programmer | Before Boss AI ADR | Open — Note: NavMesh API stable, custom may be needed for lane constraints |

---

