## Cross-GDD Review Report
Date: 2026-05-06
GDDs Reviewed: 6 (game-concept.md, systems-index.md, game-state-phase-state-machine.md, health-stamina-damage-rules.md, map-and-spawn-director.md, npc-soul-data-model.md)
Systems Covered: Game State / Phase State Machine, NPC/Soul Data Model, Map & Spawn Director, Health/Stamina & Damage Rules

---

### Consistency Issues

#### Blocking (must resolve before architecture begins)
None found. All issues are WARNING level.

#### Warnings (should resolve, but won't block)

🔴 **Dependency Asymmetry: Game State ↔ Health/Stamina**
- **GDDs involved**: game-state-phase-state-machine.md, health-stamina-damage-rules.md
- **Issue**: Health says "Game State / Phase State Machine: Depends on this AND this depends on it (Hard)". But Game State's dependency list does NOT include Health/Stamina. Either Game State is missing Health in its dependency list, or Health is wrong about Game State depending on it.
- **Resolution**: Add Health/Stamina & Damage Rules to Game State's dependency list as "Depends on this (Hard)", or remove the "Depends on this" claim from Health's dependency entry.

🔴 **Dependency Asymmetry: Game State ↔ NPC/Soul Data Model**
- **GDDs involved**: game-state-phase-state-machine.md, npc-soul-data-model.md
- **Issue**: NPC says "Game State / Phase State Machine: Depends on this AND this depends on it (Hard)". But Game State's dependency list does NOT include NPC/Soul Data Model.
- **Resolution**: Add NPC/Soul Data Model to Game State's dependency list as "Depends on this (Hard)", or remove the "Depends on this" claim from NPC's dependency entry.

🔴 **Dependency Asymmetry: NPC ↔ Map & Spawn Director**
- **GDDs involved**: npc-soul-data-model.md, map-and-spawn-director.md
- **Issue**: Map says "NPC/Soul Data Model: This depends on it (Hard)" (Map depends on NPC). But NPC's dependency list does NOT include Map & Spawn Director.
- **Resolution**: Add Map & Spawn Director to NPC's dependency list as "Depends on this (Hard)" (Map depends on NPC), or remove the "This depends on it" claim from Map's dependency entry.

🔴 **Formula Incompatibility: Ward Drain / Hallucination Multiplier**
- **GDDs involved**: map-and-spawn-director.md, health-stamina-damage-rules.md
- **Issue**: Two different formulas for the same "effective ward drain":
  - Map: `effective_ward_drain = base_drain_rate * (1 + relic_count * time_drain_multiplier)` with `time_drain_multiplier` range 0.1-1.0
  - Health: `effective_ward_drain = base_drain_rate * hallucination_multiplier` with `hallucination_multiplier` range 1.0-3.0 (sourced from Map)
- **Resolution**: Unify the formula. If Map owns the multiplier, Health should use Map's formula. The naming (`time_drain_multiplier` vs `hallucination_multiplier`) and ranges (0.1-1.0 vs 1.0-3.0) must also be reconciled.

🔴 **Data Mismatch: Strike Penalty Value**
- **GDDs involved**: map-and-spawn-director.md, health-stamina-damage-rules.md
- **Issue**: Map says `StrikeTimePenaltySec = 30` (default), but Health says "Searchlight Strike: 20.0 per unresolved hit". If these refer to the same penalty, the values contradict.
- **Resolution**: Unify the strike penalty value. Either Map's `StrikeTimePenaltySec` should be 20 (matching Health), or Health's Searchlight Strike penalty should be 30 (matching Map). Also ensure the same config variable is referenced in both GDDs.

---

### Game Design Issues

#### Blocking
None found.

#### Warnings

⚠️ **Potential Cognitive Load Risk**
- **GDDs involved**: All (when fully designed)
- **Issue**: During NightSurvival, the player may need to manage 5-6 active systems simultaneously: Ward timer (Health), cover/hazards (Map), movement (Player Controller), boss avoidance (Boss Chase AI), resource use (Resource Effects), objective tracking (Shrine Objective). Research suggests 3-4 simultaneous active systems is the comfort limit.
- **Note**: Current GDDs only cover 2 systems (Health + Map). Full assessment pending more GDDs. Consider which systems can be made passive or simplified.
- **Recommendation**: When designing Player Controller, Boss Chase AI, Resource Effects, and Shrine Objective, explicitly categorize each as "active" (player must manage) or "passive" (system runs automatically). Aim for ≤4 active systems during NightSurvival.

---

### Cross-System Scenario Issues

Scenarios walked: 2
1. Strike Hit (Map + Health)
2. Day→Night Transition (Game State + NPC + Consequence Resolver + Map + Health)

#### Blockers
None.

#### Warnings

⚠️ **Strike Hit** — Map & Spawn Director, Health/Stamina & Damage Rules
- **Step**: Map strike hit → Health penalty application
- **Issue**: Data flow mismatch. Map says strike penalty is `strike_time_penalty_sec` (default 30), but Health says Searchlight Strike penalty is 20.0. Which value is actually applied?
- **Recommendation**: Unify the strike penalty value and ensure both GDDs reference the same config variable.

#### Info

ℹ️ **Day→Night Transition** — Game State, NPC, Consequence Resolver, Map, Health
- **Note**: `day_penalties_sec` in Health's `initial_ward_sec` formula is listed as "DaySimulation output", but the Day Service & Selection GDD hasn't been written yet. The source of day penalties is currently undefined.
- **Recommendation**: When designing Day Service & Selection, ensure it produces `day_penalties_sec` that feeds into Health's night-start formula.

---

### GDDs Flagged for Revision

| GDD | Reason | Type | Priority |
|-----|--------|------|----------|
| game-state-phase-state-machine.md | Missing Health/Stamina and NPC/Soul Data Model in dependency list | Consistency | Warning |
| health-stamina-damage-rules.md | Formula mismatch with Map (ward drain); strike penalty value mismatch | Consistency | Warning |
| map-and-spawn-director.md | Formula mismatch with Health (ward drain); strike penalty value mismatch; missing from NPC dependency list | Consistency | Warning |
| npc-soul-data-model.md | Missing Map & Spawn Director in dependency list | Consistency | Warning |

---

### Verdict: CONCERNS

CONCERNS: 5 consistency warnings found that should be resolved before architecture begins, but they are not blocking (all GDDs are still In Design/In Review). No blocking issues.

### If not FAIL — recommended actions:
1. Fix dependency asymmetries: Update Game State GDD to include Health and NPC in its dependency list. Update NPC GDD to include Map in its dependency list.
2. Unify ward drain formula: Decide whether Map or Health owns the formula, and ensure both GDDs reference the same formula and variable names.
3. Unify strike penalty value: Ensure Map and Health agree on the Searchlight Strike penalty value (30 or 20).
4. Continue designing remaining MVP GDDs (Player Controller, Day Service & Selection, Consequence Resolver, etc.).

---

### Entity Registry Note
Entity registry (`design/registry/entities.yaml`) is empty — consistency checks relied on full GDD reads only. Run `/consistency-check` after this review to populate the registry.

