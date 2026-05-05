# Game State / Phase State Machine

> **Status**: In Design
> **Author**: User + Copilot
> **Last Updated**: 2026-05-06
> **Implements Pillar**: Consequence-driven survival loop with day/night emotional contrast

## Overview

Game State / Phase State Machine is the authoritative runtime controller for Solar Phobia's core loop. It transitions the run through Day Service, Choice Lock, Night Survival, and Resolve/Reset phases, and enforces which systems are active in each phase so rules cannot overlap incorrectly. This system exists to guarantee that player decisions in daytime are committed before nighttime begins, making consequence timing immediate and reliable. Without it, the game's consequence-driven identity breaks because selection, curse resolution, and survival pressure could desynchronize.

## Player Fantasy

This system should make the player feel the weight of commitment: once Day choices are finalized, the world shifts and there is no rewind inside the run. The emotional promise is ownership - "night happens because of what I decided" - not random punishment. It also reinforces contrast: daytime feels deliberate and bounded, nighttime feels urgent and irreversible. When functioning correctly, players mentally connect failure to prior decisions rather than unfair randomness.

## Detailed Design

### Core Rules

1. A run starts in `DayService`.
2. During `DayService`, only daytime actions are legal: inspect souls, assign tea/incense/offering, confirm save choices.
3. Transition to `ChoiceLock` occurs only when exactly 2 souls are marked saved (or configured minimum) and the player confirms.
4. `ChoiceLock` is atomic: unsaved soul is finalized; Consequence Resolver computes curse payload for night.
5. Transition to `NightSurvival` only after curse payload and night spawn data are valid.
6. During `NightSurvival`, daytime selection UI/actions are disabled; survival systems are enabled (movement, hazards, chase, shrine objective, resource consumption).
7. Transition to `Resolve` occurs on shrine reached (success) or death/failure (fail).
8. `Resolve` records outcome metrics, then transitions to `Reset`.
9. `Reset` clears run-scoped state and returns to `DayService` for next loop.
10. Any subsystem update request checks current phase contract; out-of-phase requests are rejected with reason codes.
11. `AutoCommitPolicy = CurrentValidElseDefault` is deterministic: (a) keep current valid selection; else (b) fill missing slots from fixed priority list `Linh -> Van -> Minh`; if still invalid, transition to `FatalError`.
12. In release builds, `FatalError` attempts safe fallback: emit failure snapshot, transition to `Resolve`, then `Reset`.

### States and Transitions

| State | Entry Condition | Exit Condition | Behavior |
|-------|-----------------|----------------|----------|
| Bootstrapping | Scene loaded | Core references valid | Initializes state machine, then enters DayService |
| DayService | Run start or Reset complete | Player confirms valid 2-of-3 selection | Enables service/selection flow only |
| ChoiceLock | DayService exit | Consequence payload + night setup ready | Commits abandoned soul, computes curse |
| NightSurvival | ChoiceLock exit | Shrine reached OR player dead | Enables hazards/chase/objective loop |
| Resolve | NightSurvival exit | Outcome persisted | Generates result snapshot |
| Reset | Resolve exit | Runtime reset finished | Clears transient state, loops to DayService |
| FatalError | Invalid critical contract | Dev: manual recovery; Release: fallback to Resolve -> Reset | Freezes progression in dev, preserves loop recoverability in release |

### Interactions with Other Systems

- **Day Service & Selection -> State Machine**: submits selection payload; receives allowed-action mask.
- **Consequence Resolver <- State Machine**: triggered on `ChoiceLock`; returns curse payload contract.
- **Night Survival Run <- State Machine**: receives start signal + payload bundle.
- **Boss Chase AI / Solar Hazard / Resource Effects <- State Machine**: phase gating + lifecycle events (`OnNightStart`, `OnNightEnd`).
- **HUD/Audio <- State Machine**: consume phase change events for UI mode and mix snapshots.
- **Save/Reset <- State Machine**: receives deterministic outcome snapshot and reset command.

## Formulas

### Phase Duration Budget

```
phase_duration = base_phase_duration * pacing_multiplier
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| base_phase_duration | float sec | 60-240 | config | Default duration per phase |
| pacing_multiplier | float | 0.5-1.5 | config/run mode | Global pacing scale |

**Expected output range**: 30-360 sec
**Edge case**: clamp output to [30, 360] sec.

### Night Start Eligibility

```
night_start_ready = has_valid_selection AND consequence_payload_ready AND spawn_bundle_ready
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| has_valid_selection | bool | {0,1} | Day Service | Exactly required saves selected |
| consequence_payload_ready | bool | {0,1} | Consequence Resolver | Curse data compiled |
| spawn_bundle_ready | bool | {0,1} | Map/Spawn Director | Night spawn points validated |

**Expected output range**: boolean
**Edge case**: if false after timeout, enter FatalError.

### Run Outcome Score (telemetry only)

```
outcome_score = shrine_reached*100 - death_penalty - night_time_seconds*0.2
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| shrine_reached | int | 0-1 | Objective system | Success marker |
| death_penalty | float | 0-100 | config | Penalty on failure |
| night_time_seconds | float sec | 0-600 | runtime | Survival duration |

**Expected output range**: -20 to 100
**Edge case**: clamp to [-50, 100] for analytics stability.

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|----------|-------------------|-----------|
| Player confirms with < required saved souls | Stay in DayService; show blocking reason `InvalidSelectionCount` | Prevents ambiguous consequence mapping |
| Consequence payload generation fails in ChoiceLock | Retry once; if still invalid -> FatalError | Night cannot start without deterministic curse |
| Two transition triggers fire same frame (e.g., shrine + death) | Resolve by priority: Death > ShrineReached; single transition only | Avoids dual outcome corruption |
| Timer expires during DayService without confirmation | Auto-commit current valid selection; if invalid, choose deterministic default policy | Keeps loop cadence and avoids deadlock |
| Night systems report active during DayService | Force-disable and emit contract violation event | Preserves phase integrity |
| Reset interrupted by missing references | In dev: enter FatalError; in release: force failure Resolve snapshot then Reset | Avoids partial-reset ghost state while preserving recoverability |

## Dependencies

| System | Direction | Nature of Dependency |
|--------|-----------|---------------------|
| Day Service & Selection | Depends on this | **Hard**: receives phase allowlist; emits `SelectionConfirmed(payload)` |
| Consequence Resolver | Depends on this | **Hard**: invoked on `ChoiceLock`; must return `CursePayload` before Night |
| Map & Spawn Director | This depends on it | **Hard**: provides validated spawn bundle for night start |
| Player Controller | Depends on this | **Hard**: movement/input mode changes by phase |
| Shrine Objective & Win/Lose Rules | Depends on this | **Hard**: transition triggers `Resolve` conditions |
| Solar Residue Hazard | Depends on this | **Hard**: enabled only in NightSurvival |
| Boss Chase AI | Depends on this | **Hard**: chase lifecycle controlled by phase events |
| Resource Effects | Depends on this | **Soft**: can precompute in day; consumed in night under phase gates |
| HUD & Diegetic Feedback | Depends on this | **Soft**: subscribes to phase-change events for presentation mode |
| Audio State Director | Depends on this | **Soft**: maps phase state to mix snapshot transitions |
| Save Seed / Run Reset | Depends on this | **Hard**: needs deterministic run result snapshot + reset command |

## Tuning Knobs

| Parameter | Current Value | Safe Range | Effect of Increase | Effect of Decrease |
|-----------|---------------|------------|-------------------|-------------------|
| DayDurationSec | 180 | 60-300 | More planning time, lower panic cadence | Faster loop, higher pressure |
| ChoiceLockTimeoutSec | 5 | 1-15 | More forgiveness before commit | Snappier but harsher commits |
| NightDurationCapSec | 360 | 120-480 | Longer survival windows | Tighter runs |
| PacingMultiplier | 1.0 | 0.5-1.5 | Slower overall tempo | Faster tempo |
| AutoCommitPolicy | `CurrentValidElseDefault` | enum | More deterministic fallback behavior | More player-dependent outcomes |
| TransitionDebounceMs | 100 | 0-300 | Fewer duplicate transitions | Risk of double-trigger |
| FatalErrorRetryCount | 1 | 0-3 | More resilience to transient failures | Faster fail-fast |
| PhaseEventDispatchMode | `OrderedReliable` | enum | Safer cross-system sync | Potentially lower throughput |

## Visual/Audio Requirements

| Event | Visual Feedback | Audio Feedback | Priority |
|-------|-----------------|----------------|----------|
| Enter DayService | Warm LUT + stable UI mode | Calm ambient bed | High |
| Enter ChoiceLock | Brief desaturation pulse + lock icon | Low drum hit + tension rise | High |
| Enter NightSurvival | Cold grade swap + vignette increase | Threat motif + louder wind/wave layer | Critical |
| Enter Resolve (Success) | Shrine glow bloom + soft fade | Relief stinger | Medium |
| Enter Resolve (Failure) | Rapid darken + camera shake settle | Distorted hit + low drone | Medium |
| Enter FatalError | Red debug overlay (dev build) | Error beep only (dev) | Low |

## UI Requirements

| Information | Display Location | Update Frequency | Condition |
|-------------|------------------|------------------|-----------|
| Current phase name/icon | Top-center | On phase change | Always |
| Allowed action hints | Context panel | On mode change | DayService/ChoiceLock |
| Choice validity state | Decision panel | Real-time | DayService |
| Night objective status (Reach Shrine) | Top-right | Real-time | NightSurvival |
| Failure reason code (if blocked) | Toast/log panel | On rejection event | Any |

## Acceptance Criteria

- [ ] Run always enters `DayService` after boot and after each reset.
- [ ] Day -> ChoiceLock transition is blocked unless required selection count is valid.
- [ ] ChoiceLock commits exactly one abandoned soul and triggers Consequence Resolver once.
- [ ] NightSurvival cannot start unless `night_start_ready == true`.
- [ ] Out-of-phase actions are rejected with explicit reason code (no silent acceptance).
- [ ] Night exit resolves to exactly one outcome (success or failure), never both.
- [ ] Resolve produces deterministic snapshot consumed by Reset.
- [ ] Reset clears run-scoped transient state and returns to DayService without stale flags.
- [ ] Phase change events are delivered in order to subscribed systems.
- [ ] Performance: phase transition processing completes within 0.5 ms average on target PC.
- [ ] Pacing validation: median Day phase duration is within 120-220 sec and median Night phase duration is within 180-360 sec in 10 representative test runs.

## Open Questions

| Question | Owner | Deadline | Resolution |
|----------|-------|----------|------------|
| Should DayService use hard timer in first playable or manual confirm only? | Game Designer | Before prototype test #1 | Open |
| Should Death-priority outcome policy be configurable by mode (default remains Death > ShrineReached)? | Systems Designer | Before content-complete milestone | Open |
| Should the fallback soul priority order (`Linh -> Van -> Minh`) be data-driven per chapter/act? | Game Designer | Before content expansion beyond vertical slice | Open |
