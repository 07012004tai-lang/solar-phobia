# Story 003: Night Phase Movement — WASD + Skills

> **Epic**: phase-state-machine
> **Status**: Ready
> **Layer**: Foundation
> **Type**: Integration
> **Manifest Version**: N/A

## Context

**GDD**: `design/gdd/game-state-phase-state-machine.md`
**Requirement**: `TR-state-002` (Day/Night phase transitions with phase-gated system activation)

**ADR Governing Implementation**: ADR-0001: Phase State Machine Architecture
**ADR Decision Summary**: R3 ReactiveProperty-based state machine with 7 states and phase contracts for action gating.

**Engine**: Unity 6000.3.11f1 (Unity 6) | **Risk**: HIGH
**Engine Notes**: Uses New Input System (required for Unity 6) — verify against Unity 6 docs.

---

## Acceptance Criteria

*From GDD `design/gdd/game-state-phase-state-machine.md`, scoped to this story:*

- [ ] WASD movement with CharacterController works in NightSurvival
- [ ] Sprint (Shift) functional
- [ ] Spirit Dash (-5s Ward) functional
- [ ] Swing (-2s Ward) functional  
- [ ] Glide (-1s/s Ward) functional

---

## Implementation Notes

*Derived from ADR-0001 Implementation Guidelines:*

- Phase contracts define which actions are allowed in NightSurvival (Move, Sprint, Dash, Swing, Glide, Crouch, InteractShrine)
- PlayerController subscribes to CurrentPhase and enables/disables movement based on phase
- Movement skills apply Ward penalties via IWardTimer interface

---

## Out of Scope

*Handled by neighbouring stories — do not implement here:*

- Story 004: Cover detection (different mechanic)
- Story 005: Boss Searchlight (different system)
- Story 009: Sensory tiers (Ward timer integration)

---

## QA Test Cases

**[For Integration stories — automated test specs]:**

- **AC-1**: WASD movement only works in NightSurvival
  - Given: CurrentPhase is DayService
  - When: Player presses WASD keys
  - Then: No movement occurs (action rejected by phase contract)
  - Edge cases: Phase transitions to NightSurvival → movement enables immediately

- **AC-2**: Sprint applies Ward penalty
  - Given: CurrentPhase is NightSurvival, player is moving
  - When: Player holds Shift to sprint
  - Then: Movement speed increases AND Ward timer decreases by sprint cost
  - Edge cases: Ward timer at 0 (sprint disabled), player in cover (sprint allowed)

- **AC-3**: Dash applies -5s Ward penalty
  - Given: CurrentPhase is NightSurvival
  - When: Player activates Spirit Dash
  - Then: Quick forward burst AND Ward decreases by 5 seconds
  - Edge cases: Ward < 5s (dash disabled), cooldown active

- **AC-4**: Swing applies -2s Ward penalty
  - Given: CurrentPhase is NightSurvival
  - When: Player activates Swing (rope/grapple)
  - Then: Forward swing motion AND Ward decreases by 2 seconds
  - Edge cases: No valid swing point (action fails silently)

- **AC-5**: Glide applies -1s/s Ward penalty
  - Given: CurrentPhase is NightSurvival, player is airborne
  - When: Player holds jump to glide
  - Then: Extended air time AND Ward decreases at 1 sec/sec
  - Edge cases: Ground contact (glide stops), Ward reaches 0 (glide stops)

---

## Test Evidence

**Story Type**: Integration
**Required evidence**: `tests/integration/phase-state-machine/night-movement_test.cs` — must exist and pass

**Status**: [ ] Not yet created

---

## Dependencies

- Depends on: Story 002 (Day Phase Mechanics) — requires NightSurvival phase to exist
- Unlocks: Story 004 (Cover Detection), Story 005 (Boss Searchlight)