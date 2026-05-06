# Consequence Resolver$

> **Status**: In Design$
> **Author**: User + opencode$
> **Last Updated**: 2026-05-06$
> **Implements Pillar**: Consequence-driven survival loop with day/night emotional contrast$

## Overview$

Consequence Resolver is the moral arithmetic of Solar Phobia's night phase—it reads the abandoned soul from Day Service's selection payload and translates that choice into a physical curse that hunts the player. The system owns the curse mapping logic, the NightOutcomeState write, and the deterministic assignment that ensures "the person you left behind becomes your nightmare." During `ChoiceLock`, it receives the `SelectionConfirmed(payload)` from Game State Machine, extracts the `abandoned_soul_id`, and writes one of {`Drag`, `Block`, `FakeShrine`} to `NightOutcomeState`. Without this system, the game's "consequence-driven" identity collapses—there's no link between who you abandoned and what hunts you at night.

## Player Fantasy$

The player should feel **"this is my fault"** — the curse that hunts them at night is a direct manifestation of the person they abandoned. There's no randomness here; the water trap, blood net, or collapsing platform exists *because* you left that specific soul behind. The fantasy is **ownership of horror** — "I made this nightmare, and now I must survive it."

Each curse should feel distinct and personal:
- **Linh (Water Trap)**: Should feel like drowning in guilt — constant, inescapable drain.
- **Van (Blood Net)**: Should feel like being hunted and restrained — sudden, sticky, suffocating.
- **Minh (Illusion)**: Should feel deceptive and terrifying — the ground itself betrays you.

The emotional promise: **"The person you left behind is now your nightmare."** The curse isn't a generic hazard; it's *that specific soul* punishing your choice.

## Detailed Design$

### Core Rules$

1. **Trigger**: Consequence Resolver is invoked by Game State Machine during `ChoiceLock` phase when `SelectionConfirmed(payload)` is received.
2. **Payload Read**: Extract `abandoned_soul_id` from the payload. This is the soul the player left behind.
3. **Curse Mapping** (deterministic):
   - `Linh` (abandoned) → `NightOutcomeState = Drag` (Water Trap — Nước Dâng).
   - `Van` (abandoned) → `NightOutcomeState = Block` (Blood Net — Lưới Máu).
   - `Minh` (abandoned) → `NightOutcomeState = FakeShrine` (Illusion — Ảo Ảnh).
4. **Model Write**: Write `NightOutcomeState` to NPC/Soul Data Model for the abandoned soul only.
5. **Curse Payload**: Send curse type, intensity, and location bias to:
   - Map & Spawn Director (spawn bias for night hazards).
   - Curse Effect Modules (visual/audio effects for night).
6. **One-Write Rule**: Consequence Resolver writes `NightOutcomeState` exactly once per run. Contradictory writes are rejected.

### States and Transitions$

| State | Entry Condition | Exit Condition | Behavior |
|-------|-----------------|----------------|----------|
| Idle | `ChoiceLock` phase + payload received | Abandoned soul mapped | Read payload; compute curse mapping using Master GDD contract |
| Writing | Curse mapped | `NightOutcomeState` written | Write outcome to NPC Model; prepare curse payload |
| Done | `NightOutcomeState` written | Payload sent to downstream systems | Send curse type to Map Director, Curse Modules |

### Interactions with Other Systems$

- **Game State / Phase State Machine -> Consequence Resolver**: Sends `SelectionConfirmed(payload)` with `abandoned_soul_id`. Triggers resolver during `ChoiceLock`.

- **NPC/Soul Data Model <-> Consequence Resolver**: 
  - Reads `SoulId`, `DaySelectionState` for the abandoned soul.
  - Writes `NightOutcomeState` = `Drag`/`Block`/`FakeShrine` for abandoned soul only.
  - Validates one-write rule (rejects duplicate writes).

- **Map & Spawn Director <- Consequence Resolver**: Receives curse type and intensity for night spawn bias:
  - `Drag` (Linh) → Mo Oan (CursedMound) placement, Water Trap zones.
  - `Block` (Van) → Lưới Máu (Blood Net) hazards, reduced visibility. 
  - `FakeShrine` (Minh) → FalseSafeMound (Ảo Ảnh) hazards, illusory platforms. 

- **Curse Effect Modules <- Consequence Resolver**: Receives curse type for night visual/audio effects:
  - `Drag` → Water sound layers, blue tint, drowning VFX. 
  - `Block` → Blood net VFX, red tint, restraint sounds. 
  - `FakeShrine` → Illusion VFX, ground collapse, deceptive platforms. 

## Formulas$

[To be designed]

## Edge Cases$

[To be designed]

## Dependencies$

[To be designed]

## Tuning Knobs$

[To be designed]

## Visual/Audio Requirements$

[To be designed]

## UI Requirements$

[To be designed]

## Acceptance Criteria$

[To be designed]

## Open Questions$

[To be designed]