# Design: Act 1 Clean Architecture Foundation

## 1. Kiến trúc tổng thể

### Domain
Chứa luật game thuần C#, không phụ thuộc Unity.
- Entity: Ghost, Order, Shrine, ActProgress, BossEncounter
- Value Object: GamePhase, OrderType, DialogueId, KindnessScore, SpiritEssence, SunExposure
- Domain Service / Rule: DialogueBranchResolver, EndingEvaluator, OrderSatisfactionRule, SunPressureRule, RouteRiskRule
- Repository contract: IGhostRepository, IDialogueRepository, IProgressRepository, IOrderRepository

### Application
Chứa use case và orchestration.
- Điều phối day/night loop
- Điều phối dialogue/order/boss/progression
- DTO / command / query
- Không đụng MonoBehaviour, GameObject, AudioSource, SceneManager

### Infrastructure
Hiện thực các thứ bên ngoài:
- Load JSON / ScriptableObject
- Save / Load
- Audio playback
- Input adapter
- Scene loading
- Data source / persistence

### Presentation
UI và gameplay view:
- HUD
- Dialogue UI
- Order UI
- Player controller view
- Boss view
- Animation hook
- MonoBehaviour, Canvas, TMP, Animator

### Composition
Bootstrap / DI / scene wiring:
- Installers
- Entry point
- Scene bootstrap
- Binding Application ↔ Infrastructure ↔ Presentation

### Shared
- Constants
- Extension methods
- Result / error type
- Utility dùng chung toàn project

---

## 2. Chuyển đổi từ docs sang Unity/C#

Tài liệu thiết kế hiện đang viết theo Godot/GDScript. Trong Unity implementation, map như sau:
- `DialogueManager` → `DialogueService` + `DialoguePresenter`
- `StallPhaseManager` → `DayNightGameCoordinator`
- `AudioManager` → `AudioPhaseService`
- `Ghost` / `Order` → entity/domain model C#
- JSON dialogue tree → `DialogueDefinition` data files
- UI scenes Godot → Unity prefabs + canvases

Nguyên tắc: tài liệu là source of truth cho design, nhưng runtime implementation phải Unity-native.

---

## 3. Strategy cho dữ liệu và scene

### Data strategy
Khuyến nghị hybrid:
- JSON: dialogue tree, branching narrative, event definitions
- ScriptableObject: tuning/config tĩnh, balance, asset references
- Save DTO: progress, flags, unlocks, ending state

### Scene strategy
Khuyến nghị cho Act 1:
- `Boot`
- `Act1_Shrine`
- `Act1_BeachRun`

Luồng scene nên dùng bootstrap/persistent root để giữ state giữa các phase.

---

## 4. State machine boundaries

### Global flow
- BOOT → DAY_HUB → DIALOGUE / ORDER → SUNSET_WARNING → NIGHT_TRAVEL → SHRINE_ARRIVAL → ENDING_EVALUATION

### Bounded responsibilities
- Global phase machine: Application
- NPC / ghost transition rules: Domain
- UI / animation response: Presentation
- Persistence / restore: Infrastructure

---

## 5. Namespace / asmdef decision

Khuyến nghị chốt rename root namespace/asmdef sang `SolarPhobia.*` cho code mới.
Lý do:
- Repo và docs đang dùng Solar Phobia làm game identity.
- `_Project` còn trống nên rename lúc này là rẻ nhất.
- Giảm lệch giữa domain, package naming, scene naming và documentation.

Nếu chưa muốn rename ngay, thì ít nhất cần một quy ước nhất quán trước khi viết file đầu tiên.

---

## 6. Risks / unknowns

- Dialogue authored bằng JSON thuần hay hybrid JSON + ScriptableObject?
- Boss encounter Act 1 mặc định là Cá Ông hay có branch sang Chú Ba theo điều kiện?
- Có bao nhiêu lần visit cho từng ghost và điều kiện unlock ending là gì?
- Game sẽ dùng 1 scene lớn hay 3 scene nhỏ như đề xuất?
- Có rename toàn bộ `TinyMonsterArena.*` ngay trong phase foundation không?

---

## 7. Implementation intent

Change này không triển khai code game hoàn chỉnh. Nó chốt nền kỹ thuật để các change sau chỉ việc đi vào implementation theo task đã rõ, giảm rủi ro rework ở giữa chừng.

---

## 8. Technology stack decisions

### Runtime UI
Khuyến nghị dùng `uGUI + TextMeshPro` cho runtime Act 1 UI, vì:
- Tài liệu narrative/UI hiện tại đã bám mô hình Canvas/Label/Portrait/Choice.
- Runtime HUD của game này là thành phần trực tiếp, nhiều trạng thái động, dễ làm hơn với uGUI trong vertical slice.
- `UI Toolkit` nên ưu tiên cho editor tooling, debug windows, hoặc công cụ nội bộ trước.

### Async / orchestration
- `UniTask`: dùng cho async flows trong `Application` và `Infrastructure` như load dialogue, chuyển scene, fade audio, đợi animation/timer.

### Dependency injection
- `VContainer`: dùng làm composition container ở `Composition` để wire use cases, repositories, services, và presenters.

### Reactive state / eventing
- `R3`: dùng cho reactive local state ở `Presentation` và các view model/state bridge khi cần push UI updates rõ ràng.
- `MessagePipe`: dùng cho cross-layer events hoặc publish/subscribe giữa feature modules khi không muốn tham chiếu trực tiếp.
- `ObservableCollections` / `ObservableCollections.R3`: dùng cho reactive collection views, danh sách order/choice/queue, và các collection cần bind theo state stream.
- Quy ước: tránh dùng cả hai cho cùng một loại signal trong cùng một subsystem; `R3` cho state stream, `MessagePipe` cho domain/application events, `ObservableCollections` cho reactive list/view binding.

### Data processing
- `ZLinq`: dùng trong `Application` / `Infrastructure` cho query/filter/map trên collections lớn hoặc hot paths.

### Animation / motion
- `DOTween`: dùng trong `Presentation` cho UI transitions, portrait fade, dialogue reveal, boss intro, sun/shadow overlay motion.

### Architectural rule of thumb
- `Domain` không phụ thuộc bất kỳ package nào ở trên.
- `Application` chỉ phụ thuộc `UniTask` / `ZLinq` nếu cần; không phụ thuộc UI runtime.
- `Presentation` được phép dùng `R3`, `MessagePipe`, `ObservableCollections`, `DOTween`, `TMP`, `Canvas`.
- `Composition` là nơi duy nhất nên biết đầy đủ stack wiring.

### Package availability note
- `UniTask`, `ZLinq`, `VContainer` hiện đã có trong `Packages/manifest.json`.
- `MessagePipe`, `R3`, `ObservableCollections`, `ObservableCollections.R3` hiện có mặt dưới `Assets/Packages/`, nhưng chưa được khai báo như UPM dependencies trong manifest.
- Nếu team muốn chuyển toàn bộ sang UPM manifest-managed dependencies, cần chốt package path/version trước khi thay đổi manifest.

