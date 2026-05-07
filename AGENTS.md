# AGENTS.md - Solar Phobia Unity Project

## Project Overview
Unity 6000.3.11f1 (Unity 6) project using C# 9.0 targeting .NET 4.7.1.
Architecture follows clean layering with Assembly Definitions: Domain → Application → Infrastructure/Presentation → Composition.

## Build & Test Commands

### Unity Editor (Primary)
- Open project in Unity 6000.3.11f1 Editor
- **Build**: File → Build Profiles (Ctrl+Shift+B), then Build
- **Test Runner**: Window → General → Test Runner (or Ctrl+Alt+T)
  - EditMode tests: Assets/_Project/**/Editor/Tests/
  - PlayMode tests: Assets/_Project/**/Tests/

### Command Line (CI/CD)
```bash
# Run EditMode tests
Unity.exe -runTests -projectPath "I:\unityVers\Solar phobia" -testPlatform EditMode -testResults results.xml

# Run PlayMode tests
Unity.exe -runTests -projectPath "I:\unityVers\Solar phobia" -testPlatform PlayMode -testResults results.xml

# Build project (Windows standalone example)
Unity.exe -quit -batchmode -projectPath "I:\unityVers\Solar phobia" -buildTarget Win64 -executeMethod BuildPipeline.BuildPlayer
```

### Run Single Test (Unity Test Runner)
- Open Test Runner window
- Right-click specific test → Run Selected
- Or use NUnit's `Category` attribute to filter test runs

## Code Style Guidelines

### Naming Conventions
- **Namespaces**: `TinyMonsterArena.Domain`, `NhemBootstrap.Editor.Steps` (PascalCase with dots)
- **Classes/Interfaces**: `BootstrapWindow`, `IBootstrapStep` (PascalCase, prefix I for interfaces)
- **Methods**: `CreateGUI`, `Init` (PascalCase)
- **Private fields**: `_steps`, `_config` (underscore + camelCase)
- **Local variables**: `tempRoot`, `snapshot` (camelCase)
- **Constants**: `Rng` (static readonly), or UPPER_SNAKE_CASE for true constants
- **Assembly Definitions**: `TinyMonsterArena.Domain.asmdef` matching namespace

### File Structure
```csharp
using System;                    // System imports first
using System.Collections.Generic;
using NhemBootStrap.Editor.Core; // Third-party/Project imports after
using UnityEditor;
using UnityEngine;

namespace NhemBootstrap.Editor.Steps {
    /// <summary>XML doc comments on public types.</summary>
    public class StepViewModel {
        // ── Section Separators ──────────────────────────────
        private List<StepViewModel> _steps;

        /// <summary>XML docs on public members.</summary>
        public void DoWork() {
            // Implementation
        }
    }
}
```

### Formatting
- **Braces**: Opening brace on new line (Allman style)
- **Indentation**: 4 spaces (no tabs)
- **Line length**: Aim for ~120 characters, but prioritize readability
- **Sections**: Use comment separators with box-drawing chars: `// ── Section Name ──────────────────────`
- **Regions**: Avoid `#region`; use section comments instead

### Types & Null Handling
- Use **nullable reference types** where applicable (C# 9.0)
- **String checks**: Use `string.IsNullOrEmpty()` or `string.IsNullOrWhiteSpace()`
- **Collections**: Prefer `List<T>` for mutable lists, `T[]` for fixed-size, `IEnumerable<T>` for returns
- **var keyword**: Use when type is obvious: `var list = new List<string>();`

### Error Handling
- Use **try-finally** for cleanup (see BootstrapPropertyTests.cs:143-146)
- Avoid empty catch blocks; log or rethrow with `throw;`
- Unity-specific: Use `Debug.LogWarning()` or `Debug.LogError()` for diagnostics
- Test assertions: NUnit `Assert.That()`, `Assert.AreEqual()`, `StringAssert.Contains()`

### Imports Organization
1. System namespaces (`System`, `System.Collections.Generic`, etc.)
2. Third-party namespaces (`NhemBootstrap`, `Cysharp`, etc.)
3. Unity namespaces (`UnityEngine`, `UnityEditor`, etc.)
4. Project namespaces (`TinyMonsterArena.Domain`, etc.)

### Testing Patterns
- **Framework**: NUnit (Unity Test Framework 1.6.0)
- **Property-based testing**: Use fixed seed RNG for reproducibility (see `new System.Random(42)`)
- **Test naming**: `MethodName_Scenario_ExpectedResult` (see BootstrapPropertyTests.cs)
- **XML docs on tests**: Include `<summary>` with "Validates: Requirements X.X" referencing specs
- **Test file location**: Mirror source structure in `Editor/Tests/` or `Tests/` folders

## Assembly Definitions (.asmdef)
- Place one `.asmdef` per layer/folder
- Set `autoReferenced: false` for explicit dependency management
- Reference other assemblies via `references` array
- Use `rootNamespace` matching assembly name

## Key Packages
- **VContainer**: Dependency injection (jp.hadashikick.vcontainer)
- **UniTask**: Async/await for Unity (com.cysharp.unitask)
- **ZLinq**: LINQ extensions (com.cysharp.zlinq)
- **DOTween**: Animation tweens (Demigiant)
- **Odin Inspector**: Editor enhancements (Sirenix)

## Scene Folder Structure

**Use only** `Assets/_Project/_Scenes/` for project scenes:

| Folder | Purpose |
|--------|---------|
| `_Scenes/Dev/` | Development, testing, and prototype scenes |
| `_Scenes/Dev/Prototype/` | Prototype test scenes |
| `_Scenes/Dev/Dialogue/` | Dialogue system development scenes |
| `_Scenes/Gameplay/` | Main gameplay and level scenes |
| `_Project/Settings/Scenes/` | URP scene templates only |

**Never use**: `Assets/Scenes/` (deprecated - delete if empty)

## Coding Standards Namespace

Use `SolarPhobia.Rules` namespace for codifiable standards:

```csharp
using SolarPhobia.Rules;

// Layer attribute for architectural enforcement
[Layer(NamingConventions.Layers.Domain)]
public class MyEntity { }

// Scene path constants
string devScenes = ScenePaths.Scenes.Dev;
```

See `Assets/_Project/Domain/Rules/` for:
- `NamingConventions.cs` - Namespace and naming rules
- `LayerAttributes.cs` - Architectural layer attributes
- `ScenePaths.cs` - Scene folder path constants

## Git Workflow
- **Ignore**: Library/, Temp/, Obj/, Build/, Logs/, UserSettings/, *.csproj, *.sln
- **Track**: Assets/, Packages/, ProjectSettings/
- See `.gitignore` for complete list

---

## Engine Version Reference

@docs/engine-reference/unity/VERSION.md
