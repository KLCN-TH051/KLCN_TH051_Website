// wwwroot/js/modules/instructor/lesson/lesson.insert.js
import LessonApi from "/js/api/lessonApi.js";
import Toast from "/js/components/Toast.js";

let currentChapterId = null;
let lessonTypesLoaded = false;

async function loadLessonTypes() {
    if (lessonTypesLoaded) return;

    const select = document.getElementById("lessonType");
    if (!select) return;

    try {
        const types = await LessonApi.getLessonTypes();
        select.innerHTML = '<option value="" disabled selected>Chọn loại bài học</option>';
        types.forEach(type => {
            const option = document.createElement("option");
            option.value = type.value;
            option.textContent = type.label;
            select.appendChild(option);
        });
        lessonTypesLoaded = true;
    } catch (err) {
        console.error("Lỗi load loại bài học:", err);
        select.innerHTML = '<option value="">Lỗi tải dữ liệu</option>';
        Toast.show("Không tải được loại bài học!", "danger");
    }
}

document.addEventListener("DOMContentLoaded", () => {
    setTimeout(initLessonInsert, 100);
});

function initLessonInsert() {
    // Mở modal thêm bài học
    document.body.addEventListener("click", async (e) => {
        const btn = e.target.closest(".lesson-insert-btn");
        if (!btn) return;

        currentChapterId = btn.dataset.chapterId;
        await loadLessonTypes();

        // Reset form
        document.getElementById("lessonName").value = "";
        document.getElementById("lessonType").selectedIndex = 0;
        document.getElementById("previewCheck").checked = false;

        // Mở modal
        bootstrap.Modal.getOrCreateInstance(document.getElementById("addLessonModal")).show();
    });

    // Nút "Tạo bài học" trong modal thêm
    document.getElementById("saveLessonBtn")?.addEventListener("click", createLesson);
}

async function createLesson() {
    const title = document.getElementById("lessonName")?.value.trim();
    const typeValue = document.getElementById("lessonType")?.value;
    const isFree = document.getElementById("previewCheck")?.checked ?? false;

    if (!title || !typeValue) {
        Toast.show("Vui lòng nhập tên và chọn loại bài học!", "danger", 4000);
        return;
    }

    const type = parseInt(typeValue);

    try {
        // BƯỚC 1: TẠO BÀI HỌC TRÊN SERVER
        const result = await LessonApi.createLesson(currentChapterId, {
            title,
            type,
            isFree
        });

        // Lấy lessonId từ response (giả sử API trả về { id: 123, ... })
        const newLessonId = result.id || result.Id || result.ID;

        // Đóng modal thêm
        bootstrap.Modal.getInstance(document.getElementById("addLessonModal")).hide();

        // BƯỚC 2: TỰ ĐỘNG MỞ MODAL CHI TIẾT THEO LOẠI
        if (type === 1) {
            // TYPE = 1 → BÀI ĐỌC → MỞ MODAL NHẬP NỘI DUNG NGAY
            window.openReadingModal(
                currentChapterId,
                newLessonId,
                title,
                "",           // content rỗng
                isFree,
                true          // isNew = true → hiển thị "Tạo bài đọc mới"
            );
        }
        else if (type === 2) {
            // TYPE = 2 → VIDEO → bạn làm sau
            Toast.show("Chức năng thêm Video sẽ làm sau!", "info");
        }
        else if (type === 3) {
            // Mở modal Quiz mới
            if (typeof window.openQuizModal === "function") {
                window.openQuizModal(
                    currentChapterId,
                    newLessonId,
                    title,
                    null,   // quiz rỗng
                    true    // isNew = true
                );
            } else {
                Toast.show("Chưa load modal Quiz!", "danger");
            }
        }



        // Cập nhật danh sách bài học (hiển thị bài mới ở cuối)
        const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
        window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);

        // Thông báo tạo thành công (trước khi vào nhập nội dung)
        Toast.show("Tạo bài học thành công! Đang mở trình soạn thảo...", "success", 3000);

    } catch (err) {
        console.error("Tạo bài học thất bại:", err);
        Toast.show("Tạo bài học thất bại!", "danger", 5000);
    }
}