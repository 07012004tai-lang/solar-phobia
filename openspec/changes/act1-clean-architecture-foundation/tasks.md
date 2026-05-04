# Tasks: Act 1 Clean Architecture Foundation

## Phase 0 - Architecture foundation
- [x] Chốt naming/namespace strategy cho code mới (`SolarPhobia.*`).
- [x] Chốt data strategy: JSON / ScriptableObject / hybrid.
- [x] Chốt scene strategy cho Act 1: Boot + Shrine + BeachRun.
- [x] Chốt UI/runtime stack: `UI Toolkit` cho runtime, `uGUI` chỉ là fallback nếu cần.
- [x] Chốt reactive/event stack: `R3` cho state stream, `MessagePipe` cho cross-layer events, `ObservableCollections` / `ObservableCollections.R3` cho reactive collection binding.
- [x] Chốt async/DI/animation stack: `UniTask`, `VContainer`, `DOTween`, `ZLinq`.
- [x] Chốt package source strategy: giữ Cysharp reactive packages dưới `Assets/Packages/` for now; manifest-managed packages vẫn dùng cho `UniTask`, `ZLinq`, `VContainer`.
- [x] Định nghĩa domain model tối thiểu cho phase, ghost, order, progress, ending.  
- [x] Định nghĩa repository contracts và service rules cho domain.
- [x] Xác định application use cases và command/query objects.
- [x] Xác định composition/bootstrap flow và dependency direction (placeholder installer created).

## Phase 1 - Vertical slice planning
- [ ] Chốt luồng playthrough tối thiểu từ shrine tới shrine.
- [ ] Chốt UI boundaries cho dialogue, order, phase HUD trên UI Toolkit.
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

