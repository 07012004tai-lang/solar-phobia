# Health/Stamina & Damage Rules

**Status:** In Design
**Scope:** Define player "Health/Stamina" formulas only (Player State). Excludes trap logic or Boss behavior.
**Based on:** "Opposites Attract" (Master GDD).

## Dependencies

- **Game State / Phase State Machine:** Depends on this (Hard). (Manages Ward lifecycle.)
- **Map & Spawn Director:** This depends on it (Hard). (Uses formulas for time coefficients and hazard damage.)

## 1. Overview: The Unified Resource

In *Solar Phobia* we merge "Health" and "Stamina" into a single resource called **Spirit Ward** (Ward Timer). It represents the player's humanity and survival time.

- **Using stamina:** Consumes Ward.
- **Touching traps (Health):** Subtracts Ward.
- **Ward reaches 0:** Game Over (the Whale devours the player).

## 2. Initialization Formula (Day → Night Translation)

```csharp
float Initial_Ward_Sec = Base_Ward_Sec + (Ghosts_Saved * Ward_Per_Ghost_Sec) - Day_Penalties_Sec;
```
- `Base_Ward_Sec` – Minimum survival time (e.g., **60.0 s**).
- `Ghosts_Saved` – Number of souls successfully offered (0‑2).
- `Ward_Per_Ghost_Sec` – Bonus time per saved soul (e.g., **30.0 s**).
- `Day_Penalties_Sec` – Penalties from Day Service & Selection (e.g., broken bowls, spilled soup).

## 3. Active Stamina Costs

```csharp
Current_Ward = Current_Ward - Action_Cost;
```
| Action | Cost (seconds) |
|--------|-----------------|
| Jump / Double Jump | 0 |
| Swing (throwing cloth) | -2.0 |
| Spirit Dash | -5.0 |
| Glide (air time) | -1.0 per second |

## 4. Ward Drain Rate & Dynamic Damage

### Ward Drain (Time Drain)
The drain rate syncs with **Map & Spawn Director** when the player picks up **Whale Bones** (Ngọc Cốt):

```csharp
float effective_ward_drain = base_drain_rate * (1 + (bones_carried * hallucination_multiplier));
```
- `base_drain_rate` = **1.0 unit/s**.
- `bones_carried` = Number of collected bones (0‑3).
- `hallucination_multiplier` = **1.0** (1 bone → 2× drain, 2 bones → 3× drain).

### Direct Damage Penalties (Trap Effects)
| Hazard | Ward Penalty (seconds) |
|--------|------------------------|
| **Nước Dâng (Linh)** – Water‑soaked trap | -3.0 per second of immersion |
| **Lưới Máu (Văn)** – Blood net | -5.0 instantly + `Speed * 0.5` debuff for 3 s |
| **Ảo Ảnh (Minh)** – Illusion | -15.0 (instant fall) |
| **Searchlight Strike** – Whale’s beam | **-20.0** (`StrikeTimePenaltySec = 20.0`) per hit without cover |

## 5. Readability Thresholds (Ward Percent Tiers)

```csharp
float Ward_Percent = (Current_Ward / Max_Initial_Ward) * 100f;
```
| Percent | Tier | Effects |
|---------|------|---------|
| > 75% | **Tier 1 – Stable** | Normal |
| ≤ 75% | **Tier 2 – Creeping Dread** | Vignette (α 0.3) & Low‑pass audio |
| ≤ 50% | **Tier 3 – Heavy Burden** | Heavy breathing, `Dash_Cooldown += 0.1s` |
| ≤ 25% | **Tier 4 – Panic** | Chromatic aberration, whispering audio |
| ≤ 10 s | **Tier 5 – Death Spiral** | Tinnitus SFX, tunnel vision |

---

*End of Health/Stamina & Damage Rules GDD.*
