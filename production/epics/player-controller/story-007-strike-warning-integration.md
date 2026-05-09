# Story 007: Strike Warning Integration — Map Director Event → Reticle Icon

> **Epic**: player-controller
> **Status**: Ready
> **Layer**: Core
> **Type**: Integration
> **Priority**: P1
> **Estimate**: 5 hours
> **Manifest Version**: N/A

## Context

**GDD**: `design/gdd/player-controller.md`
**TR-ID**: `TR-player-009`
**ADR Governing Implementation**: ADR-0003: Player Controller Pattern
**ADR Decision Summary**: Player Controller receives `OnStrikeWarning` from Map Director and displays warning icon near reticle. Icon must have z-order priority above Tier 4 panic effects (chromatic aberration). Player position + collider bounds sent to Map Director each frame during NightSurvival.

**Engine**: Unity 6000.3.11f1 (Unity 6) | **Risk**: HIGH

---

## Acceptance Criteria

- [ ] On receiving `OnStrikeWarning` from Map Director, warning icon appears near reticle.
- [ ] Warning icon z-order is above Tier 4 panic effects (chromatic aberration, screen shake).
- [ ] Multiple overlapping strike warnings display only the nearest/highest-priority icon (no UI clutter).
- [ ] Warning icon clears when strike resolves — no lingering icon after strike ends.
- [ ] Player position + collider bounds sent to Map Director each frame during `NightSurvival`.

---

## Implementation Notes

*Derived from ADR-0003 Implementation Guidelines:*

- Subscribe to `IMapSpawnDirector.OnStrikeWarning` observable
- Warning icon: Screen Space — Overlay canvas, highest sort order
- Send position via `IMapSpawnDirector.UpdatePlayerPosition(Vector3, Bounds)` each frame in `Update()`
- Open question (GDD): 3D world-space vs 2D screen-space icon — default to 2D screen-space until UX decision resolved
- Multiple warnings: track list, display only nearest by distance to player

---

## Out of Scope

*Handled by neighbouring stories — do not implement here:*

- Strike damage calculation (owned by Map & Spawn Director)
- Ward Timer penalty on strike hit (owned by Ward Timer)
- Strike warning visual design / animation (Visual/Feel story)

---

## QA Test Cases

- **AC-1**: OnStrikeWarning shows icon
  - Given: Phase = NightSurvival; player exposed (not in cover)
  - When: `OnStrikeWarning` event received from Map Director
  - Then: Warning icon visible near reticle

- **AC-2**: Icon z-order above panic effects
  - Given: Sensory tier = DeathSpiral (chromatic aberration active)
  - When: Strike warning fires
  - Then: Warning icon renders above chromatic aberration layer

- **AC-3**: Icon clears after strike resolves
  - Given: Warning icon is showing
  - When: Strike resolves (Map Director fires `OnStrikeResolved`)
  - Then: Warning icon hidden

- **AC-4**: Multiple warnings → single icon
  - Given: Two simultaneous strike warnings
  - When: Both `OnStrikeWarning` events received
  - Then: Only one icon shown (nearest strike)

- **AC-5**: Position sent to Map Director
  - Given: Phase = NightSurvival
  - When: Each frame
  - Then: `IMapSpawnDirector.UpdatePlayerPosition` called with current position + bounds

---

## Test Evidence

**Story Type**: Integration
**Required evidence**: `tests/integration/player-controller/strike-warning_test.cs` OR documented playtest in `production/qa/evidence/strike-warning-playtest.md`

**Status**: [ ] Not yet created

---

## Dependencies

- Depends on: Story 004 (Cover Detection), Map & Spawn Director epic (partial — `IMapSpawnDirector` interface)
- Unlocks: None (leaf story)
