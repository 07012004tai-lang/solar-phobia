# Systems Index: Solar Phobia: Nắng Gắt

> **Status**: Draft
> **Created**: 2026-05-06
> **Last Updated**: 2026-05-06
> **Source Concept**: design/gdd/game-concept.md

---

## Overview

Solar Phobia's mechanical scope centers on a consequence-driven day/night loop: choose who to save in a constrained daytime decision phase, then survive the night while facing direct gameplay consequences from the person left behind. The minimum system map prioritizes loop orchestration, consequence transformation, and readable survival pressure over content breadth. This index decomposes those needs into implementable systems, dependencies, and design order for the vertical slice.

---

## Systems Enumeration

| # | System Name | Category | Priority | Status | Design Doc | Depends On |
|---|-------------|----------|----------|--------|------------|------------|
| 1 | Game State / Phase State Machine | Core | MVP | Approved | design/gdd/game-state-phase-state-machine.md | — |
| 2 | NPC/Soul Data Model | Narrative | MVP | In Review | design/gdd/npc-soul-data-model.md | — |
| 3 | Map & Spawn Director | Core | MVP | Not Started | — | — |
| 4 | Health/Stamina & Damage Rules | Core | MVP | Not Started | — | — |
| 5 | Player Controller | Core | MVP | Not Started | — | Game State / Phase State Machine, Map & Spawn Director |
| 6 | Shrine Objective & Win/Lose Rules | Core | MVP | Not Started | — | Game State / Phase State Machine, Player Controller, Health/Stamina & Damage Rules |
| 7 | Resource Effects | Economy | Vertical Slice | Not Started | — | NPC/Soul Data Model, Game State / Phase State Machine |
| 8 | Solar Residue Hazard | Gameplay | Vertical Slice | Not Started | — | Map & Spawn Director, Health/Stamina & Damage Rules, Game State / Phase State Machine |
| 9 | Day Service & Selection | Gameplay | MVP | Not Started | — | NPC/Soul Data Model, Resource Effects, Game State / Phase State Machine |
| 10 | Consequence Resolver | Gameplay | MVP | Not Started | — | Day Service & Selection, NPC/Soul Data Model, Game State / Phase State Machine |
| 11 | Curse Effect Modules (inferred) | Gameplay | MVP | Not Started | — | Consequence Resolver, Player Controller, Map & Spawn Director, Health/Stamina & Damage Rules |
| 12 | Boss Chase AI | Gameplay | Vertical Slice | Not Started | — | Player Controller, Map & Spawn Director, Game State / Phase State Machine, Health/Stamina & Damage Rules |
| 13 | Night Survival Run | Gameplay | MVP | Not Started | — | Shrine Objective & Win/Lose Rules, Solar Residue Hazard, Boss Chase AI, Curse Effect Modules (inferred), Resource Effects |
| 14 | HUD & Diegetic Feedback (inferred) | UI | Vertical Slice | Not Started | — | Day Service & Selection, Night Survival Run, Health/Stamina & Damage Rules, Resource Effects |
| 15 | Audio State Director (inferred) | Audio | Vertical Slice | Not Started | — | Game State / Phase State Machine, Boss Chase AI, Night Survival Run |
| 16 | Save Seed / Run Reset (inferred) | Persistence | Alpha | Not Started | — | Game State / Phase State Machine, NPC/Soul Data Model, Map & Spawn Director, Day Service & Selection, Consequence Resolver, Night Survival Run |

---

## Categories

| Category | Description | Typical Systems |
|----------|-------------|-----------------|
| **Core** | Foundation systems everything depends on | Loop state machine, player controller, objective/win-lose logic |
| **Gameplay** | The systems that make the game fun | Day selection, consequences, curses, hazards, chase pressure |
| **Economy** | Resource creation and consumption | Tea/incense/offering effects and conversion |
| **Persistence** | Save state and continuity | Run reset and deterministic restart |
| **UI** | Player-facing information displays | Choice feedback, danger readability, objective cues |
| **Audio** | Sound and music systems | Day/night mix state, threat escalation cues |
| **Narrative** | Story and identity state delivery | NPC/soul identity and abandonment state |

---

## Priority Tiers

| Tier | Definition | Target Milestone | Design Urgency |
|------|------------|------------------|----------------|
| **MVP** | Required for the core loop to function. Without these, you can't test "is this fun?" | First playable prototype | Design FIRST |
| **Vertical Slice** | Required for one complete, polished area. Demonstrates the full experience. | Vertical slice / demo | Design SECOND |
| **Alpha** | All features present in rough form. Complete mechanical scope, placeholder content OK. | Alpha milestone | Design THIRD |
| **Full Vision** | Polish, edge cases, nice-to-haves, and content-complete features. | Beta / Release | Design as needed |

---

## Dependency Map

### Foundation Layer (no dependencies)

1. Game State / Phase State Machine — orchestrates day -> choice -> night transitions for all systems.
2. NPC/Soul Data Model — provides stable identity/state for selection and consequence transformation.
3. Map & Spawn Director — anchors where hazards, boss, shrine, and player enter each phase.
4. Health/Stamina & Damage Rules — provides shared survivability contract for hazards and chase.

### Core Layer (depends on foundation)

1. Player Controller — depends on: Game State / Phase State Machine, Map & Spawn Director.
2. Shrine Objective & Win/Lose Rules — depends on: Game State / Phase State Machine, Player Controller, Health/Stamina & Damage Rules.
3. Resource Effects — depends on: NPC/Soul Data Model, Game State / Phase State Machine.
4. Solar Residue Hazard — depends on: Map & Spawn Director, Health/Stamina & Damage Rules, Game State / Phase State Machine.
5. Day Service & Selection — depends on: NPC/Soul Data Model, Resource Effects, Game State / Phase State Machine.

### Feature Layer (depends on core)

1. Consequence Resolver — depends on: Day Service & Selection, NPC/Soul Data Model, Game State / Phase State Machine.
2. Curse Effect Modules — depends on: Consequence Resolver, Player Controller, Map & Spawn Director, Health/Stamina & Damage Rules.
3. Boss Chase AI — depends on: Player Controller, Map & Spawn Director, Game State / Phase State Machine, Health/Stamina & Damage Rules.
4. Night Survival Run — depends on: Shrine Objective & Win/Lose Rules, Solar Residue Hazard, Boss Chase AI, Curse Effect Modules, Resource Effects.

### Presentation Layer (depends on features)

1. HUD & Diegetic Feedback — depends on: Day Service & Selection, Night Survival Run, Health/Stamina & Damage Rules, Resource Effects.
2. Audio State Director — depends on: Game State / Phase State Machine, Boss Chase AI, Night Survival Run.

### Polish Layer (depends on everything)

1. Save Seed / Run Reset — depends on: core gameplay state contracts and loop outcomes.

---

## Recommended Design Order

| Order | System | Priority | Layer | Agent(s) | Est. Effort |
|-------|--------|----------|-------|----------|-------------|
| 1 | Game State / Phase State Machine | MVP | Foundation | game-designer | M |
| 2 | NPC/Soul Data Model | MVP | Foundation | game-designer | S |
| 3 | Map & Spawn Director | MVP | Foundation | game-designer | S |
| 4 | Health/Stamina & Damage Rules | MVP | Foundation | systems-designer | S |
| 5 | Player Controller | MVP | Core | gameplay-programmer, game-designer | M |
| 6 | Shrine Objective & Win/Lose Rules | MVP | Core | game-designer | S |
| 7 | Day Service & Selection | MVP | Core | game-designer, ui-programmer | M |
| 8 | Consequence Resolver | MVP | Feature | systems-designer, game-designer | M |
| 9 | Curse Effect Modules | MVP | Feature | systems-designer, gameplay-programmer | M |
| 10 | Night Survival Run | MVP | Feature | game-designer, gameplay-programmer | L |
| 11 | Resource Effects | Vertical Slice | Core | systems-designer | S |
| 12 | Solar Residue Hazard | Vertical Slice | Core | systems-designer, technical-artist | M |
| 13 | Boss Chase AI | Vertical Slice | Feature | ai-programmer | M |
| 14 | HUD & Diegetic Feedback | Vertical Slice | Presentation | ui-programmer, ux-designer | M |
| 15 | Audio State Director | Vertical Slice | Presentation | sound-designer, audio-director | S |
| 16 | Save Seed / Run Reset | Alpha | Polish | gameplay-programmer | S |

---

## Circular Dependencies

- None found.

---

## High-Risk Systems

| System | Risk Type | Risk Description | Mitigation |
|--------|-----------|------------------|------------|
| Consequence Resolver | Design | If curse mapping is unclear, choices feel arbitrary instead of meaningful. | Prototype 3 fixed curse mappings early and run fast playtests for readability. |
| Night Survival Run | Scope | Combines hazards, chase, curses, and objective timing; can over-expand quickly. | Hard-lock one map and one completion condition for slice; defer variants. |
| Boss Chase AI | Technical | Poor pacing/tuning can make night either trivial or unfair. | Start with simple pursuit states and tune via fixed benchmark runs. |

---

## Progress Tracker

| Metric | Count |
|--------|-------|
| Total systems identified | 16 |
| Design docs started | 2 |
| Design docs reviewed | 2 |
| Design docs approved | 1 |
| MVP systems designed | 1/10 |
| Vertical Slice systems designed | 0/5 |

---

## Next Steps

- [ ] Review and approve this systems enumeration
- [ ] Design MVP-tier systems first (use `/design-system [system-name]`)
- [ ] Run `/design-review` on each completed GDD
- [ ] Run `/gate-check pre-production` when MVP systems are designed
- [ ] Prototype the highest-risk system early (`/prototype [system]`)
