# Epic: Player Controller & Skills

> **Layer**: Core
> **GDD**: design/gdd/player-controller.md
> **Architecture Module**: PlayerController (from architecture.md)
> **Status**: Ready
> **Stories**: Not yet created — run `/create-stories player-controller`

## Overview

Player Controller is the input and movement system that translates player actions into game responses, enforcing what the player can and cannot do based on the current phase. During Day phase, the player is restricted to UI-only interactions—selecting NPCs, assigning resources, and confirming choices—with no physical movement allowed. During Night phase, the system enables WASD movement, sprint, and contextual interaction as the player navigates the lane from start shrine to end shrine.

**Theme**: "Ownership through contrast" — Daytime calm makes nighttime panic feel deserved, not random. When the player survives or fails, they should connect it to their earlier choices, not to unfair mechanics.

## Governing ADRs

| ADR | Decision Summary | Engine Risk |
|-----|-----------------|-------------|
| ADR-0003: Player Controller Pattern | New Input System + CharacterController with phase-gated enable/disable | HIGH |

## GDD Requirements

| TR-ID | Requirement | ADR Coverage |
|-------|-------------|--------------|
| TR-player-001 | WASD movement with sprint, dash, glide, swing actions (phase-gated) | ADR-0003 ✅ |

## Definition of Done

This epic is complete when:
- All stories are implemented, reviewed, and closed via `/story-done`
- All acceptance criteria from `design/gdd/player-controller.md` are verified
- All Logic and Integration stories have passing test files in `tests/`
- All Visual/Feel and UI stories have evidence docs with sign-off in `production/qa/evidence/`

## Next Step

Run `/create-stories player-controller` to break this epic into implementable stories.