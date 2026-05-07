# Story 004: Cover Detection — Mound Collider

> **Epic**: phase-state-machine
> **Status**: Ready
> **Layer**: Foundation
> **Type**: Integration
> **Manifest Version**: N/A

## Context

**GDD**: `design/gdd/game-state-phase-state-machine.md`
**Requirement**: `TR-state-002` (Day/Night phase transitions with phase-gated system activation)

**ADR Governing Implementation**: ADR-0001: Phase State Machine Architecture
**ADR Decision Summary**: R3 ReactiveProperty-based state machine with 7 states and phase contracts.

**Engine**: Unity 6000.3.11f1 (Unity 6) | **Risk**: HIGH

---

## Acceptance Criteria

*From GDD `design/gdd/game-state-phase-state-machine.md`, scoped to this story:*

- [ ] Cover detection requires full collider inside Mound collider

---

## Implementation Notes

*Derived from ADR-0001 Implementation Guidelines:*

- Player collider must be fully contained within Mound collider for valid cover
- Three Mound types: SafeMound (MoThuong), CursedMound (MoOan), FalseSafeMound
- PlayerController subscribes to PhaseStateMachine.CurrentPhase to enable/disable cover detection

---

## QA Test Cases

**[For Integration stories — automated test specs]:**

- **AC-1**: Full collider inside Mound = valid cover
  - Given: Player collider bounds are fully within Mound collider bounds
  - When: Boss searchlight sweep passes over Mound area
  - Then: Player is considered "in cover" and not hit by strike
  - Edge cases: Partial overlap (edge of collider), player at Mound edge

- **AC-2**: Partial collider outside Mound = exposed
  - Given: Player collider extends beyond Mound bounds
  - When: Boss searchlight sweeps over player
  - Then: Player is "exposed" and receives strike warning
  - Edge cases: Standing just outside (exposed), leaning into cover (exposed)

- **AC-3**: Different Mound types provide cover
  - Given: Player is fully inside various Mound types
  - When: Searchlight sweeps
  - Then: SafeMound, CursedMound, FalseSafeMound all provide valid cover
  - Edge cases: FalseSafeMound looks safe but may have caveats (per GDD)

---

## Test Evidence

**Story Type**: Integration
**Required evidence**: `tests/integration/phase-state-machine/cover-detection_test.cs` — must exist and pass

**Status**: [ ] Not yet created

---

## Dependencies

- Depends on: Story 003 (Night Phase Movement)
- Unlocks: Story 005 (Boss Searchlight)