# Tasks: Act 1 Clean Architecture Foundation

## Phase 0 - Architecture foundation
- [ ] Chốt naming/namespace strategy cho code mới (`SolarPhobia.*` hoặc giữ tạm `TinyMonsterArena.*`).
- [ ] Chốt data strategy: JSON / ScriptableObject / hybrid.
- [ ] Chốt scene strategy cho Act 1: Boot + Shrine + BeachRun.
- [ ] Chốt UI/runtime stack: `uGUI + TMP` cho runtime, `UI Toolkit` cho editor/tooling, hoặc migration plan nếu muốn runtime UI Toolkit.
- [ ] Chốt reactive/event stack: `R3` cho state stream, `MessagePipe` cho cross-layer events, `ObservableCollections` / `ObservableCollections.R3` cho reactive collection binding.
- [ ] Chốt async/DI/animation stack: `UniTask`, `VContainer`, `DOTween`, `ZLinq`.
- [ ] Chốt package source strategy: package nào đi qua `Packages/manifest.json` và package nào đang nằm dưới `Assets/Packages/`.
- [ ] Định nghĩa domain model tối thiểu cho phase, ghost, order, progress, ending.
- [ ] Định nghĩa repository contracts và service rules cho domain.
- [ ] Xác định application use cases và command/query objects.
- [ ] Xác định composition/bootstrap flow và dependency direction.

## Phase 1 - Vertical slice planning
- [ ] Chốt luồng playthrough tối thiểu từ shrine tới shrine.
- [ ] Chốt UI boundaries cho dialogue, order, phase HUD.
- [ ] Chốt audio responsibilities giữa infrastructure và presentation.
- [ ] Chốt persistence requirements cho progress, kindness, unlocks.

## Phase 2 - Story slice planning
- [ ] Chốt scope NPC chính: Ông Văn, Em Linh, Anh Minh, Chú Ba.
- [ ] Chốt rules cho branching dialogue, kindness score, ending evaluator.
- [ ] Chốt criteria unlock boss variant / secret path.

## Phase 3 - Implementation readiness
- [ ] Tạo danh sách file/folder cụ thể cho Domain/Application/Infrastructure/Presentation/Composition.
- [ ] Tạo acceptance checklist cho Act 1 playable end-to-end.
- [ ] Ghi lại open questions cần quyết định trước khi code.

## Completion criteria
- [ ] Proposal, design, tasks đều đã sẵn sàng cho change implementation.
- [ ] Không còn ambiguity lớn về layer boundary hoặc data flow.
- [ ] Có roadmap đủ rõ để chuyển sang `/opsx:apply` hoặc một change implement tiếp theo.

