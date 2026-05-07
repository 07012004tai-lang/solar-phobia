# Story 005: Boss Searchlight — Sweep + Strike

> **Epic**: phase-state-machine
> **Status**: Ready
> **Layer**: Foundation
> **Type**: Integration
> **Manifest Version**: N/A

## Context

**GDD**: `design/gdd/game-state-phase-state-machine.md`
**Requirement**: `TR-state-002` (Day/Night phase transitions with phase-gated system activation)

**ADR Governing Implementation**: ADR-0001: Phase State Machine Architecture
**Engine**: Unity 6000.3.11f1 (Unity 6) | **Risk**: HIGH

---

## Acceptance Criteria

*From GDD `design/gdd/game-state-phase-state-machine.md`, scoped to this story:*

- [ ] Boss Searchlight sweep pattern scans lane
- [ ] Exposed players trigger telegraph
- [ ] Strike applies -30s Ward penalty

---

## Implementation Notes

*Derived from ADR-0001 Implementation Guidelines:*

- Boss searchlight active only in NightSurvival phase
- Searchlight sweep pattern: cone scans across playable lane
- If player not in valid cover when sweep hits → telegraph → strike
- Strike applies -30s to Ward timer

---

## QA Test Cases

**[For Integration stories — automated test specs]:**

- **AC-1**: Searchlight sweeps across lane in pattern
  - Given: CurrentPhase is NightSurvival
  - When: Boss searchlight activates
  - Then: Cone sweeps left-to-right across playable area in predictable pattern
  - Edge cases: Multiple sweeps simultaneously (not per GDD), sweep speed

- **AC-2**: Exposed player receives strike warning
  - Given: Player is NOT in valid cover (Story 004)
  - When: Searchlight cone passes over player position
  - Then: Strike telegraph triggers (warning visual + audio)
  - Edge cases: Player enters cover during telegraph (warning clears)

- **AC-3**: Strike applies -30s Ward penalty
  - Given: Telegraph period expires with player still exposed
  - When: Strike executes
  - Then: Ward timer decreases by 30 seconds AND screen shake + red flash
  - Edge cases: Ward < 30 (drops to 0 = death), multiple strikes stack

---

## Test Evidence

**Story Type**: Integration
**Required evidence**: `tests/integration/phase-state-machine/boss-searchlight_test.cs` — must exist and pass

**Status**: [ ] Not yet created

---

## Dependencies

- Depends on: Story 004 (Cover Detection)
- Unlocks: Story 009 (Sensory Tiers — strike is one trigger)