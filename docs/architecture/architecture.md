# Solar Phobia — Master Architecture

## Document Status
- Version: 1.1
- Last Updated: 2026-05-07
- Engine: Unity 6000.3.11f1 (Unity 6)
- GDDs Covered: 11 system GDDs (game-state, player-controller, day-service, shrine-objective, consequence-resolver, shadow-spatial, health-stamina, map-spawn, npc-soul, day-night-camera, game-concept)
- Cross-System Issues: RESOLVED (4 blockers fixed 2026-05-07)
- ADRs Referenced: 3 (ADR-0001, ADR-0002, ADR-0003)

---

## Engine Knowledge Gap Summary

**Risk Level**: HIGH — Unity 6 is beyond LLM training data (~2023 cutoff)

### HIGH RISK Domains
- **VContainer** — New DI package, post-cutoff
- **R3** — New reactive system, post-cutoff  
- **UI Toolkit** — Significant changes from uGUI
- **New Input System** — Required for Unity 6, not in training data

### Systems in HIGH RISK
- Phase State Machine → uses VContainer + R3
- NPC/Soul Data Model → uses R3 observables
- Player Controller → New Input System
- All other systems depend on these

⚠️ **Flagged**: All ADRs created from this architecture must include Engine Compatibility sections.

---

## System Layer Map

```
┌─────────────────────────────────────────────┐
│  PRESENTATION LAYER                         │  ← UI, HUD, audio, VFX
├─────────────────────────────────────────────┤
│  FEATURE LAYER                              │  ← gameplay systems, AI
├─────────────────────────────────────────────┤
│  CORE LAYER                                 │  ← physics, input, movement
├─────────────────────────────────────────────┤
│  FOUNDATION LAYER                           │  ← engine integration, state
├─────────────────────────────────────────────┤
│  PLATFORM LAYER                             │  ← Unity engine APIs
└─────────────────────────────────────────────┘
```

### Layer Assignments

| Layer | Systems |
|-------|---------|
| **Foundation** | Phase State Machine, Soul Data Repository, Map Spawn Director, Ward Timer, Shadow Manager, Camera Director, Save/Reset |
| **Core** | Player Controller, Day Selection UI, Shrine Objective, Physical Crowding, Tactile Rituals, Sensory Feedback |
| **Feature** | Consequence Resolver, Curse Effects, Boss Searchlight, Relic Collector, Night Survival Run |
| **Presentation** | HUD-less Design, Sensory Feedback Manager, Audio Mix Director |

---

## Module Ownership

### Foundation Layer

| Module | Owns | Exposes | Consumes |
|--------|------|---------|----------|
| **PhaseStateMachine** | Phase state, transitions, allowed-action masks | CurrentPhase (R3), OnPhaseChanged events | Game state data |
| **SoulDataRepository** | Soul entities (3 souls), saved/abandoned state | Souls, OnSoulStateChanged | None |
| **MapSpawnDirector** | Map chunks, spawn points, hazard placement, seed | GetSpawnPoints(), GenerateChunk() | Phase state |
| **WardTimer** | Ward value, drain rate, sensory tiers | CurrentWard (R3), OnWardDepleted | Phase state, SoulData |
| **ShadowManager** | Shadow polygon geometry, shrink state | ShadowBounds, OnShrinkComplete | Phase state |
| **CameraDirector** | Camera position, FOV, transitions | CameraTransform, OnTransitionComplete | Phase state |
| **SaveResetManager** | Run snapshot, deterministic seed | Save(), Load(), Reset() | All modules |

### Core Layer

| Module | Owns | Exposes | Consumes |
|--------|------|---------|----------|
| **PlayerController** | Player position, input, movement | Position, IsGrounded, OnInteract | PhaseStateMachine, InputSystem |
| **DaySelectionUI** | Selection state, ritual assignments | OnSelectionConfirmed | SoulDataRepository |
| **ShrineObjective** | Shrine zone, win/lose detection | OnShrineReached, OnPlayerDied | WardTimer, PhaseStateMachine |
| **TactileRituals** | Minigame state (diêm/rót/vay) | OnRitualComplete, HươngHỏaAwarded | None |

### Feature Layer

| Module | Owns | Exposes | Consumes |
|--------|------|---------|----------|
| **ConsequenceResolver** | Curse payload generation | GetCursePayload(sacrificedId) | DaySelectionUI, SoulData |
| **CurseEffectManager** | Active curse, hazard spawning | ApplyCurse(), RemoveCurse() | ConsequenceResolver, MapSpawn |
| **BossSearchlight** | Boss position, sweep, strike | OnSweepStart, OnStrikeHit | PlayerController |
| **RelicCollector** | Ngọc Cốt pickups | OnRelicPicked, CurrentMultiplier | WardTimer |
| **NightSurvivalRun** | Night loop state | OnNightStart, OnNightEnd | All Core systems |

### Presentation Layer

| Module | Owns | Exposes | Consumes |
|--------|------|---------|----------|
| **SensoryFeedbackManager** | Visual/audio tiers | UpdateTier(wardPercent) | WardTimer |
| **AudioMixDirector** | Audio snapshots | SetPhaseSnapshot() | PhaseStateMachine |

---

## Data Flow

### 1. Frame Update Path
```
Input System (New Input)
    → PlayerController.ProcessInput()
    → CharacterController.Move()
    → Player Position Updated
    → CameraDirector.Update()
    → Render
```

### 2. Event/Signal Path (R3 Reactive)
```
PhaseStateMachine._currentPhase
    .Subscribe(phase => {
        if (phase == PhaseState.NightSurvival)
            NightSurvivalRun.Enable();
    });

SoulDataRepository._souls
    .Where(s => s.IsSaved)
    .Subscribe(savedCount => 
        WardTimer.Initialize(savedCount));
```

### 3. Save/Load Path
```
PhaseStateMachine.GetSnapshot()
    → { Phase, Seed, SoulsSaved, SacrificeId, WardValue }

SaveResetManager.Save(snapshot)
    → PlayerPrefs.SetString("run_seed", JsonUtility.ToJson(snapshot))

Load → Restore all module states from snapshot
```

### 4. Initialisation Order
```
1. PhaseStateMachine (bootstrap)
2. SoulDataRepository (load persisted)
3. MapSpawnDirector (generate initial chunk)
4. CameraDirector (set initial position)
5. [Wait for player input] → Enable DayService
```

---

## API Boundaries

### IPhaseStateMachine
```csharp
public interface IPhaseStateMachine {
    ReadOnlyReactiveProperty<PhaseState> CurrentPhase { get; }
    IObservable<PhaseTransition> OnPhaseChanged { get; }
    
    void TransitionTo(PhaseState newPhase);
    bool TryTransition(PhaseState newPhase);
    bool IsActionAllowed(GameAction action);
}
```

### ISoulRepository
```csharp
public interface ISoulRepository {
    IReadOnlyList<Soul> Souls { get; }
    Soul GetSoul(string id);
    void SetSoulState(string id, SoulState state);
    IReadOnlyList<Soul> GetSavedSouls();
}
```

### IWardTimer
```csharp
public interface IWardTimer {
    ReactiveProperty<float> CurrentWard { get; }
    ReadOnlyReactiveProperty<SensoryTier> CurrentTier { get; }
    IObservable<Unit> OnDepleted { get; }
    
    void Initialize(int ghostsSaved, int dayPenalties);
    void ApplyPenalty(float seconds);
    void SetDrainMultiplier(float multiplier);
}
```

### IPlayerController
```csharp
public interface IPlayerController {
    Vector3 Position { get; }
    bool IsGrounded { get; }
    bool IsSprinting { get; }
    
    void Enable();  // Phase-gated
    void Disable(); // Phase-gated
    void ApplyForce(Vector3 force);
    event Action<string> OnInteract; // "swap", "shove", "shrine", "interact"
}
```

---

## ADR Audit

**No ADRs exist yet.**

All technical requirements from GDDs currently have **no architectural decision** supporting them.

### Traceability Coverage

| Requirement ID | GDD | Requirement | ADR Coverage | Status |
|---------------|-----|-------------|--------------|--------|
| TR-state-001 | game-state-phase-state-machine.md | Phase state machine | — | ❌ GAP |
| TR-state-002 | game-state-phase-state-machine.md | Day/Night transitions | — | ❌ GAP |
| TR-player-001 | player-controller.md | WASD movement | — | ❌ GAP |
| TR-ward-001 | health-stamina-damage-rules.md | Ward initialization | — | ❌ GAP |
| TR-npc-001 | npc-soul-data-model.md | Soul entity storage | — | ❌ GAP |
| TR-map-001 | map-and-spawn-director.md | Procedural generation | — | ❌ GAP |

---

## Required ADRs (Priority Order)

### Foundation Layer (Must create first)
1. **Phase State Machine Architecture** — VContainer + R3 reactive pattern
   - Covers: TR-state-001, TR-state-002, TR-state-003
2. **Event Bus Architecture** — MessagePipe pub/sub vs direct coupling
3. **Soul Data Repository Pattern** — Dictionary-based storage with R3 observables

### Core Layer (Before implementation)
4. **Player Controller Pattern** — CharacterController + New Input System
5. **Ward Timer Implementation** — ReactiveProperty with sensory tiers
6. **Map Generation Strategy** — Deterministic seed + chunk spawning
7. **Shrine Objective Logic** — Win/lose detection with phase gating

### Feature Layer
8. **Consequence Resolver Pattern** — Curse payload generation from sacrifice
9. **Boss Searchlight AI** — Sweep pattern + cover detection
10. **Curse Effect Modules** — Karma-based hazard spawning

### Presentation Layer
11. **Sensory Feedback System** — HUD-less tier-based visual/audio feedback

---

## Architecture Principles

1. **Phase-Gated Activation** — Only systems required by current phase are active. PlayerController disabled during DayService, DaySelectionUI disabled during NightSurvival.

2. **Deterministic Reproducibility** — All randomness seeded. Map generation, spawn placement, and run outcomes must be reproducible from seed.

3. **Single Lethal Resource** — Ward Timer is the only death condition. No health, no other counters. All feedback (sensory tiers) derives from this one source.

4. **Reactive Data Flow** — R3 ReactiveProperty for all state that multiple systems observe. No polling, no direct coupling.

5. **Consequence Immediacy** — Day choice → Night consequence is direct and visible. Karma hazards spawn based on exact sacrifice ID, not abstract "bad choices."

---

## Open Questions

| Question | Owner | Deadline | Notes |
|-----------|-------|----------|-------|
| Rendering pipeline (URP/HDRP) | Art Director | Before Feature ADRs | Affects lighting/shader patterns |
| Physics 2D vs 3D | Gameplay Programmer | Before Player Controller ADR | Game uses 2D side-scrolling but Unity 3D physics |
| Multiplayer consideration | Not MVP | Deferred | Single-player only for vertical slice |

---

## Next Steps

1. Run `/architecture-decision "Phase State Machine Architecture"` — Foundation ADR #1
2. Run `/architecture-decision "Soul Data Repository Pattern"` — Foundation ADR #2  
3. Run `/architecture-decision "Player Controller Pattern"` — Core ADR #1
4. Run `/gate-check pre-production` when all Foundation + Core ADRs are written