# Technical Preferences

> **Status**: DRAFT — Needs population
> **Purpose**: Define performance budgets and technical constraints for the project
> **Reference**: AGENTS.md Build & Test Commands

---

## Performance Budgets

### Frame Rate
| Platform | Target FPS | Acceptable Range |
|----------|------------|------------------|
| Windows | 60 | 55-60 |
| macOS | 60 | 55-60 |
| Console (target) | 60 | 55-60 |

### Load Times
| Metric | Target | Maximum |
|--------|--------|---------|
| Cold start to main menu | < 5s | 10s |
| Scene transition (day→night) | < 2s | 4s |
| Save/load game | < 1s | 3s |

### Memory
| Metric | Target | Maximum |
|--------|--------|---------|
| Runtime memory (PC) | < 2GB | 3GB |
| Asset memory budget | TBD | TBD |

---

## Rendering

- **Pipeline**: Universal Render Pipeline (URP)
- **Resolution**: Native with dynamic resolution scaling if needed
- **Post-processing**: On (Bloom, Vignette for sensory feedback)

---

## Input

- **Input System**: New Input System (InputActions)
- **Keyboard/Mouse**: Primary
- **Controller**: Future consideration

---

## Code Quality

- **Architecture**: Clean Architecture (Domain → Application → Infrastructure → Presentation)
- **Dependency Injection**: VContainer
- **Testing**: NUnit via Unity Test Framework

---

## Naming Conventions

Reference: `Assets/_Project/Domain/Rules/NamingConventions.cs`

---

## Build Targets

- Windows (primary)
- macOS (secondary)
- Future: Console platforms

---

## TODO

- [ ] Fill in actual performance measurements from profiling
- [ ] Set asset memory budgets based on target platforms
- [ ] Define specific quality settings for each tier
- [ ] Document platform-specific constraints