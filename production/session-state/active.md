# Active Session State

## Completed Sections`
- Open Questions ✅ (4 questions: shrink type, thermal min, crowd push, player movement)

## All 8 Sections Complete ✅`
- Overview, Player Fantasy, Detailed Design (Core Rules, States, Interactions)
- Formulas, Edge Cases, Dependencies, Tuning Knobs
- Visual/Audio, UI Requirements, Acceptance Criteria, Open Questions
- Acceptance Criteria ✅ (polygon shrink, thermal death, crowd density, player immunity, performance, cross-system, UI)
- UI Requirements ✅ (shadow size bar, souls remaining, crowd warning, no direct UI)
- Visual/Audio Requirements ✅ (5 events: shadow active, shrink, sunlight death, crowd density, day end)
- Tuning Knobs ✅ (initial area, shrink rate, polygon color, interacts with Game State)
- Dependencies ✅ (upstream: Game State only, downstream: None yet)
- Edge Cases ✅ (6 scenarios: polygon min size, soul pushes player, crowd density, WASD ignored, timeout freeze, dual sunlight death)
- Formulas ✅ (shrink rate, thermal death check, crowd density)
- Interactions with Other Systems ✅ (Game State phase events, Player disabled, Day Service crowd pressure, NPC thermal death)
- Interactions with Other Systems ✅ (Game State phase events, Player disabled, Day Service crowd pressure, NPC thermal death)
- States and Transitions ✅ (3 states: Active, Locked, Inactive)
- Core Rules ✅ (active phase, shadow polygon, shrink, thermal death, crowding, capacity)
- Player Fantasy ✅ (trapped, morally burdened, sun as threat, uncomfortable push)
- Overview ✅ (shadow polygon shrink, thermal death, moral pressure)

## Current Task`
Shadow Spatial Management GDD ✅ (8/8 sections written, APPROVED by design-review))

## Status`
- All 8 required sections complete: Overview, Player Fantasy, Detailed Design (Core Rules, States, Interactions), Formulas, Edge Cases, Dependencies, Tuning Knobs, Visual/Audio, UI Requirements, Acceptance Criteria, Open Questions.
- Verdict: APPROVED (design-review run)

## Files Modified`
- design/gdd/shadow-spatial-management.md (all sections written, APPROVED)
- design/gdd/systems-index.md (Shadow Spatial → Approved, tracker updated: started 6, reviewed 5, approved 3, MVP 6/16)

## Progress Tracker (from systems-index)`
- Total systems: 21
- Design docs started: 5 (+Consequence Resolver)
- Design docs reviewed: 4 (+Consequence Resolver)
- Design docs approved: 2 (+Consequence Resolver)
- MVP systems designed: 5/16 (+Consequence Resolver)
- Vertical Slice systems designed: 0/4

## Next Steps`
- Continue MVP systems: Day/Night Camera Transition (#6), Physical Crowding & Push (#8), Tactile Rituals (#9)
- Run `/gate-check pre-production` when 10+ MVP systems designed
- Run `/prototype [system]` for high-risk systems (Shadow Spatial, Consequence Resolver)`

## Session Extract — /review-all-gdds 2026-05-06
- Verdict: CONCERNS
- GDDs reviewed: 10
- Flagged for revision: day-service-and-selection.md, consequence-resolver.md, game-state-phase-state-machine.md (blocking); health-stamina-damage-rules.md, map-and-spawn-director.md, player-controller.md, shrine-objective-win-lose-rules.md (warnings)
- Blocking issues: 4 (Unowned knobs ownership, Hard dep on non-existent GDD, stale status fields, spawn validation missing)
- Recommended next: Fix 4 blocking issues → re-run /review-all-gdds → /gate-check pre-production
- Report: design/gdd/gdd-cross-review-2026-05-06.md

## Blocking Issues Fixed ✅
1. ✅ Stale status fields: day-service-and-selection.md, consequence-resolver.md headers updated to "Approved"
2. ✅ Hard dependency: day-service-and-selection.md Resource Effects downgraded to Soft
3. ✅ Spawn validation: game-state-phase-state-machine.md edge cases updated with retry x3 → FatalError
4. ✅ Interface ownership: map-and-spawn-director.md and health-stamina-damage-rules.md now declare Map & Spawn Director owns `base_drain_rate`, `hallucination_multiplier`, `StrikeTimePenaltySec`
- Systems index updated: all 3 GDDs remain "Approved" (blocking issues resolved)
- Next: Re-run /review-all-gdds to verify, or continue to next system

## Session Extract — /review-all-gdds Verification 2026-05-07
- Verdict: PASS (upgraded from CONCERNS)
- GDDs verified: 5 (the ones that were fixed)
- All 4 blocking issues confirmed resolved:
  - ✅ day-service-and-selection.md: Status = Approved
  - ✅ consequence-resolver.md: Status = Approved
  - ✅ game-state-phase-state-machine.md: Status = Approved, spawn validation fallback in place (line 107)
  - ✅ health-stamina-damage-rules.md: Interface Ownership section present
  - ✅ map-and-spawn-director.md: Interface Ownership section present
- Recommended next: Run /gate-check pre-production to validate Systems Design phase gate
- Report: design/gdd/gdd-cross-review-2026-05-06.md (original, still valid)

## Gate Check: Systems Design → Technical Setup 2026-05-07
- Verdict: PASS
- Required artifacts: 2/2 present
- Quality checks: 3/3 passing
- 3 GDDs approved with all 8 sections
- Stage updated: production/stage.txt = "Technical Setup"

## Create Architecture 2026-05-07
- Tech stack: VContainer, R3, MessagePipe, ZlinQ, ObserverCollections, DOTween, UIToolkit, New Input System
- Architecture doc: docs/architecture/architecture.md (already exists, updated status)
- 21 systems mapped across 5 layers (Platform → Foundation → Core → Feature → Presentation)
- 16 Required ADRs identified for Foundation + Core + Feature + Presentation layers
- Architecture principles: Phase-gated processing, deterministic consequences, single lethal resource (Ward Timer)

## ADR Created 2026-05-07
- ADR-0001: Phase State Machine Architecture (Accepted)
- Covers: TR-state-001 through TR-state-010 (10 requirements)
- Tech: VContainer + R3 ReactiveProperty + MessagePipe pub/sub

- ADR-0002: NPC/Soul Data Model and Phase-Locked Writes (Accepted)
- Covers: TR-npc-001 through TR-npc-009 (9 requirements)
- Tech: VContainer + Dictionary-based repo + R3 observables

- ADR-0003: Save Seed and Run Reset Strategy (Accepted)
- Covers: Deterministic seed, serialization, reset, outcome snapshots
- Tech: PlayerPrefs + JSON serialization

- ADR-0004: Ward Timer System - Single Lethal Resource (Accepted)
- Covers: TR-health-001 through TR-health-009 (9 requirements)
- Tech: VContainer + R3 ReactiveProperty

- ADR-0005: Map & Spawn Director - Lane Generation and Hazard Placement (Accepted)
- Covers: TR-map-001 through TR-map-010 (10 requirements)
- Tech: Procedural generation with deterministic seed

- ADR-0006: Player Controller - Input System and Movement (Accepted)
- Covers: Movement, sprint, cover, E-interact, phase gating
- Tech: New Input System + CharacterController + VContainer
- Core Layer ADRs complete!

## GDD Update 2026-05-07
- Updated: game-state-phase-state-machine.md
- Added: Lore foundation (Tú, Cá Ông, Mặt Trời Rỗng), Day phase timeline (5-min), Swap/Shove mechanics, Night phase platformer details, Karma hazards (Lưới Máu, Vũng Nước, Bệ Đá), Ngọc Cốt system, Ward Timer formulas, Sensory tier system (HUD-less)
- Next: Run /propagate-design-change to check ADR impact

## Completed Systems GDDs ✅`
1. Game State / Phase State Machine (Approved)
2. Player Controller & Skills (Approved)
3. Day Service & Selection (Approved)
4. Shrine Objective & Win/Lose Rules (Approved)
5. Consequence Resolver (Approved)
6. Shadow Spatial Management (Approved)

## MVP Progress`
- Total MVP systems: 16`
- MVP designed: 6/16 (+Shadow Spatial Management)
- MVP approved: 6/16 (+Shadow Spatial Management)

## File`
design/gdd/shadow-spatial-management.md (approved)
- Continue designing remaining MVP systems: Shadow Spatial Management, Day/Night Camera Transition, Physical Crowding & Push, Tactile Rituals, Resource Effects & Hương Hỏa, Curse Effect Modules, Boss Cá Ông Searchlight, Night Survival Run
- Run `/gate-check pre-production` when MVP systems are designed
- Run `/prototype [system]` for high-risk systems (Consequence Resolver, Night Survival Run)


## Setup Engine 2026-05-07
- Engine: Unity 6000.3.11f1 (Unity 6)
- Knowledge Risk: HIGH (beyond LLM training data)
- Reference Docs: Created full set (VERSION.md, breaking-changes.md, deprecated-apis.md, current-best-practices.md, modules/ui-toolkit.md)
- AGENTS.md: Updated with engine reference import
- Tech Prefs: Updated with Unity/C# naming conventions, performance budgets
- Agent Roster: Added version awareness protocol for unity-specialist
- Next: Can now run /create-architecture and write ADRs with engine-verified patterns


## ADR Created 2026-05-07
- ADR-0001: Phase State Machine Architecture (Accepted)
- Covers: TR-state-001 through TR-state-010 (10 requirements)
- Tech: VContainer + R3 ReactiveProperty + MessagePipe pub/sub
- Next: Continue with more ADRs

## Session Extract — /dev-story 2026-05-07
- Story: production/epics/phase-state-machine/story-001-day-phase-timeline.md — Day Phase Timeline (4 Pressure Phases)
- Files changed: DayPhaseTimelineService.cs, TimelinePhase.cs, DayPhaseTimelineTests.cs
- Test written: DayPhaseTimelineTests.cs (24 test functions)
- Blockers: None
- Next: /code-review [files] then /story-done [story-path]
