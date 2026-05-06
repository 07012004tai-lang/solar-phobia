# Health/Stamina & Damage Rules

> **Status**: In Review
> **Author**: User + Copilot
> **Last Updated**: 2026-05-06
> **Implements Pillar**: Psychological pressure through readable survival penalties

## Overview

Health/Stamina & Damage Rules defines how player survivability is represented, depleted, and recovered across the day/night loop, with special emphasis on Night Phase pressure readability. In this vertical slice, Night survivability uses a single lethal resource (`Ward Timer`) with no separate HP or stamina pools. Damage events from hazards, boss strikes, and costly movement actions convert directly into timer penalties, keeping UI simple and consequence legible. This system exists to ensure every mistake has a clear survival cost without diluting the game's psychological core into arcade attrition.

## Player Fantasy

This system should make players feel that survival is slipping through their fingers in real time. The intended emotion is panic-with-agency: mistakes are punishing, but readable and recoverable through smart routing and cover use. A single Ward Timer keeps stakes emotionally direct and easy to parse while still forcing hard movement trade-offs. When tuned correctly, failure feels like consequence of choices, not hidden math or unfair burst damage.

## Detailed Design

### Core Rules

1. During `DayService` and `ChoiceLock`, survivability damage processing is disabled (non-lethal phase).
2. On `NightSurvival` start, initialize `WardTimerSec` from phase config and active modifiers.
3. `WardTimerSec` is the only lethal resource in Night. If it reaches 0, trigger failure transition.
4. There is no separate HP pool and no separate stamina pool in Night; action costs and damage both consume `WardTimerSec`.
5. Hazard and boss-strike events convert to timer penalties (flat or scaled), never HP loss.
6. Bone Relic carry applies continuous Time Drain multiplier to Ward Timer decay.
7. Mobility pressure is expressed by increased action cost/cooldown at low ward thresholds, not by a second meter.
8. Shrine entry freezes timer decay and hands off to Resolve.
9. All penalty sources emit reason-coded events for HUD/telemetry.
10. Safe-zone state blocks penalty application.

### States and Transitions

| State | Entry Condition | Exit Condition | Behavior |
|-------|-----------------|----------------|----------|
| Inactive | Day/Choice phases | Night starts | No survivability ticking |
| NightActive | Night starts | WardTimer <= 0 OR shrine reached | Timer drains, penalties apply |
| HeavyBurden | WardPercent <= 50 | WardPercent > 50 | Adds recovery delay/cooldown pressure |
| PanicState | WardPercent <= 25 | WardPercent > 25 | Strong sensory distortion + higher execution risk |
| SafeZone | Player in shrine volume | Leaves safe zone or phase ends | Timer and penalties suspended |
| Failed | WardTimer <= 0 | Resolve transition | Emits failure reason snapshot |

### Interactions with Other Systems

- **Game State / Phase State Machine**: owns activation window and fail/success transitions.
- **Map & Spawn Director**: supplies strike/hazard penalties and Time Drain triggers.
- **Boss Chase AI / Solar Residue Hazard**: emit timed penalty events consumed by this system.
- **Player Controller**: reads ward thresholds to apply movement recovery/cooldown penalties.
- **HUD & Diegetic Feedback**: reads WardTimer, WardPercent tiers, and penalty reason stream.
- **Shrine Objective & Win/Lose Rules**: consumes `WardTimer <= 0` and shrine-safe signals.

## Formulas

### Initial Ward from Day-to-Night Translation

```
initial_ward_sec = base_ward_sec + (ghosts_saved * ward_per_ghost_sec) - day_penalties_sec
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| base_ward_sec | float sec | 30-180 | config | Minimum survival time at night start |
| ghosts_saved | int | 0-2 | DaySimulation output | Number of souls successfully served |
| ward_per_ghost_sec | float sec | 10-60 | config | Bonus ward per saved soul |
| day_penalties_sec | float sec | 0-30 | DaySimulation output | Day mistakes converted to ward loss |

**Expected output range**: 30-240 sec  
**Edge case**: clamp to `min_initial_ward_sec` and `max_initial_ward_sec`.

### Active Action Cost

```
ward_after_action = ward_before_action - action_cost_sec
```

| Action | action_cost_sec (default) | Notes |
|--------|---------------------------|-------|
| Jump / Double Jump | 0.0 | Baseline movement is free |
| Swing | 2.0 | Per successful tether cast |
| Spirit Dash | 5.0 | Emergency burst |
| Glide | 1.0/sec | Continuous while held |

**Edge case**: actions that would drop below zero still execute once, then resolve failure immediately.

### Passive Damage Penalties

```
ward_after_damage = max(0, ward_before_damage - damage_penalty_sec)
```

| Source | Penalty Type | damage_penalty_sec (default) |
|--------|--------------|------------------------------|
| Linh's Cursed Tide | Damage-over-time | 3.0 per second submerged |
| Van's Blood Net | Flat hit | 5.0 on contact |
| Mirage fall fail | Flat hit + checkpoint reset | 15.0 |
| Searchlight Strike | Heavy flat hit | 20.0 per unresolved hit |

### Multi-Source Penalty Cap (Fairness Guard)

```
applied_penalty_sec = min(sum(raw_penalties_sec_in_frame), max_penalty_per_frame_sec)
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| raw_penalties_sec_in_frame | float sec | >= 0 | runtime | Sum of all strike/hazard/action penalties in same frame |
| max_penalty_per_frame_sec | float sec | 4-30 | survivability config | Hard cap to prevent single-frame delete |

**Order**: cap is applied after source aggregation and before final `ward_after_damage` calculation.

### Hallucination / Relic Multiplier

```
effective_ward_drain = base_drain_rate * hallucination_multiplier
```

| Variable | Type | Range | Source | Description |
|----------|------|-------|--------|-------------|
| base_drain_rate | float/sec | 0.1-5.0 | runtime/config | Baseline ward decay |
| hallucination_multiplier | float | 1.0-3.0 | Map & Spawn Director | Increases decay while carrying relics |

**Cross-reference**: `hallucination_multiplier` is authored in `map-and-spawn-director.md`.

### Readability Thresholds

```
ward_percent = (current_ward_sec / max_initial_ward_sec) * 100
```

| Tier | Trigger | Gameplay/Sensory Effect |
|------|---------|--------------------------|
| Tier 1: Stable | `ward_percent > 75` | No penalty effects |
| Tier 2: Creeping Dread | `ward_percent <= 75` | Vignette + lowpass |
| Tier 3: Heavy Burden | `ward_percent <= 50` | Breath stress + `dash_cooldown += 0.1s` |
| Tier 4: Panic | `ward_percent <= 25` | Chromatic aberration + whisper layer |
| Tier 5: Death Spiral | `current_ward_sec <= 10` | Tunnel vision + tinnitus warning |

**Edge case**: tier transitions are debounced for 0.25 sec to avoid flicker near boundaries.

## Edge Cases

| Scenario | Expected Behavior | Rationale |
|----------|-------------------|-----------|
| Initial ward computes below minimum due to day penalties | Clamp to `min_initial_ward_sec`; emit warning metric | Prevents unwinnable night starts |
| Searchlight hit and relic drain tick in same frame | Apply strike penalty first, then continuous drain | Deterministic and debuggable order |
| Ward reaches exactly 0 inside shrine trigger frame | Shrine-safe check resolves first; if inside safe zone, do not fail | Avoids unfair frame-race deaths |
| Player repeatedly toggles glide to bypass per-second cost | Bill cost using accumulated airtime slices (no free toggling) | Prevents cost exploit |
| Rapid threshold oscillation near 25%/50% | Debounce tier transitions for 0.25 sec | Avoids feedback flicker and control jitter |
| Multiple damage sources overlap (tide + net + strike) | Sum penalties with per-frame cap (`max_penalty_per_frame_sec`) | Prevents single-frame delete while keeping danger |
| Checkpoint reset after mirage fall with <=0 ward | Resolve fail first; no checkpoint revive | Keeps death rule consistent |

## Dependencies

| System | Direction | Nature of Dependency |
|--------|-----------|---------------------|
| Game State / Phase State Machine | Depends on this and this depends on it | **Hard**: phase activation gates survivability processing; this system emits fail/safe outcomes |
| Map & Spawn Director | Depends on this and this depends on it | **Hard**: sends strike/hazard/relic drain events; consumes ward viability constraints |
| Boss Chase AI | Depends on this | **Hard**: strike consequences consume ward penalties defined here |
| Solar Residue Hazard | Depends on this | **Hard**: hazard contacts map to timer penalties and debuffs |
| Shrine Objective & Win/Lose Rules | Depends on this | **Hard**: consumes `WardTimer <= 0` failure signal and safe-zone immunity |
| Player Controller | Depends on this | **Hard**: receives threshold-tier movement penalties/cooldown pressure |
| HUD & Diegetic Feedback | Depends on this | **Soft**: displays WardTimer, tiers, and penalty reason codes |
| Audio State Director | Depends on this | **Soft**: maps ward tiers to panic audio stack |

Interface contract events:
- `OnWardInitialized(initial_ward_sec)`
- `OnWardPenaltyApplied(source, penalty_sec, remaining_sec)`
- `OnWardTierChanged(old_tier, new_tier)`
- `OnWardDepleted(reason)`
- `OnWardSafeZoneChanged(is_safe)`

## Tuning Knobs

| Parameter | Current Value | Safe Range | Effect of Increase | Effect of Decrease |
|-----------|---------------|------------|-------------------|-------------------|
| BaseWardSec | 60 | 30-180 | More survival margin | Higher fail pressure |
| WardPerGhostSec | 30 | 10-60 | Stronger day-play reward | Weaker day-night linkage |
| MaxDayPenaltySec | 10 | 0-30 | Harsher day mistakes | More forgiving day errors |
| TimeDrainMultiplier | 1.0 | 1.0-3.0 | Faster death spiral with relics | Softer relic risk |
| SearchlightPenaltySec | 20 | 5-60 | More punishment for exposure | Lower threat impact |
| TideDotPerSec | 3.0 | 0.5-8.0 | Water zones become lethal faster | Water hazard less relevant |
| BloodNetFlatPenaltySec | 5.0 | 1.0-20.0 | Contact mistakes hurt more | Net hazard less punishing |
| MirageFallPenaltySec | 15.0 | 5.0-40.0 | Platform read errors become costly | Safer recovery from falls |
| DashWardCostSec | 5.0 | 1.0-10.0 | Discourages panic dashing | Encourages aggressive mobility |
| MaxPenaltyPerFrameSec | 12.0 | 4.0-30.0 | Prevents burst delete less often | Smoother fairness guard |

### Constant Ownership

| Constant | Owner | Default | Notes |
|----------|-------|---------|-------|
| min_initial_ward_sec | Health/Stamina & Damage Rules config | 30 | Minimum survivable night start |
| max_initial_ward_sec | Health/Stamina & Damage Rules config | 180 | Reference cap for percent tier calculations |
| max_penalty_per_frame_sec | Health/Stamina & Damage Rules config | 12 | Frame-level fairness cap across overlapping penalties |

## Visual/Audio Requirements

| Event | Visual Feedback | Audio Feedback | Priority |
|-------|-----------------|----------------|----------|
| Ward tier enters 75% | Light vignette onset | Subtle lowpass blend | High |
| Ward tier enters 50% | Heavier edge darkening | Audible strained breathing | High |
| Ward tier enters 25% | Chromatic fringing + shake micro-pulse | Whisper layer activation | Critical |
| Ward <= 10s | Tunnel vision + near-monochrome stress tint | Tinnitus + heartbeat spike | Critical |
| Ward penalty applied | Brief timer flash and source color cue | Short source-tagged impact cue | Medium |

## UI Requirements

| Information | Display Location | Update Frequency | Condition |
|-------------|------------------|------------------|-----------|
| Ward timer value/progress | Primary survival HUD element | Real-time | Night phase |
| Current ward tier state | Timer ring styling | On threshold change | Night phase |
| Last penalty source | Small event ribbon near timer | On penalty event | Night phase |
| Action cost preview (Dash/Swing/Glide) | Ability hint row | On ability ready/use | Night phase |
| Safe zone status | Top-center status tag | On zone enter/exit | Night phase |

## Acceptance Criteria

- [ ] Night starts with `initial_ward_sec` matching day translation formula output.
- [ ] Ward depletion to 0 always triggers failure transition exactly once.
- [ ] Searchlight, tide, net, and mirage penalties apply configured timer loss values.
- [ ] No separate HP meter is processed during Night Phase.
- [ ] Action costs (Swing/Dash/Glide) consistently reduce ward according to config.
- [ ] Tier transitions (75/50/25/<=10s) trigger correct sensory package without flicker.
- [ ] Safe zone suppresses incoming penalties while active.
- [ ] First relic pickup is survivable on baseline route when movement execution is valid.
- [ ] Penalty reason events are emitted for every ward loss source.
- [ ] Performance: survivability update + threshold evaluation stays <= 0.2 ms average/frame on target PC.

## Open Questions

| Question | Owner | Deadline | Resolution |
|----------|-------|----------|------------|
| Should dash cost scale up in Panic tier, or remain fixed for readability? | Systems Designer | Before full combat pass | Open |
| Should Ward tier thresholds be global or route-specific per chapter? | Game Designer | Before Act 2 scope lock | Open |
| Should player receive one-time grace window at Ward <= 5s for jam difficulty mode? | Creative Director | Before external demo build | Open |
