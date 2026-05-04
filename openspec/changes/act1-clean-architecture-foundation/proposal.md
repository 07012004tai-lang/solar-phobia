# Proposal: Act 1 Clean Architecture Foundation

## Tên change
`act1-clean-architecture-foundation`

## Mục tiêu
Thiết lập nền tảng kiến trúc cho Act 1 của Solar Phobia theo clean architecture, tách rõ domain logic, orchestration, data access, presentation và composition. Change này nhằm làm cho game có thể phát triển theo từng phase mà không bị phụ thuộc chéo giữa UI, logic và dữ liệu narrative.

## Vì sao cần change này
- Hiện `_Project` mới có cấu trúc layer nhưng chưa có domain/app logic thật sự.
- Tài liệu thiết kế hiện có nội dung rất rõ, nhưng đang ở dạng Godot/GDScript; cần chuyển thành blueprint Unity/C#.
- Act 1 có nhiều hệ thống giao nhau: dialogue, order, kindness, phase day/night, boss, ending.
- Nếu không chốt kiến trúc sớm, các hệ thống này sẽ bị hard-code trong UI hoặc scene, dẫn đến khó bảo trì và khó mở rộng sang Act 2.

## Phạm vi
### In scope
- Thiết kế và chốt kiến trúc layer cho Unity project.
- Định nghĩa domain model tối thiểu cho Act 1.
- Định nghĩa các use case và orchestration chính cho day/night loop.
- Chốt chiến lược dữ liệu cho dialogue, order, progress, save/load.
- Chốt scene strategy và composition/bootstrap flow.
- Xác định backlog implementation theo phase.

### Out of scope
- Không triển khai gameplay code hoàn chỉnh trong change này.
- Không làm thêm nội dung Act 2/Act 3.
- Không làm polishing art/audio đầy đủ.
- Không tối ưu hoá production build hay release pipeline ở mức cuối cùng.

## Success criteria
- Có kiến trúc rõ ràng: `Domain → Application → Infrastructure/Presentation → Composition`.
- Có danh sách module/file cần implement cho Act 1.
- Có roadmap theo phase: foundation → vertical slice → story slice → polish.
- Có ranh giới rõ giữa data, logic, scene, UI, audio, và persistence.
- Có các câu hỏi mở được ghi lại để chốt trước khi implement.

## Kết luận đề xuất
Đây là một foundation change cho toàn bộ Act 1. Sau khi chốt, team có thể đi tiếp sang change implement bằng `/opsx:apply` mà không phải tranh luận lại kiến trúc ở từng system riêng lẻ.

