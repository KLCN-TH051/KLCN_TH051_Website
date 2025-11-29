// wwwroot/js/modules/instructor/lesson/lesson.insert.js
import LessonApi from "/js/api/lessonApi.js";
import Toast from "/js/components/Toast.js";

let currentChapterId = null;
let lessonTypesLoaded = false;

// ==================== LOAD LESSON TYPES ====================
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

// ==================== INIT ====================
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

// ==================== CREATE LESSON ====================
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

        const newLessonId = result.id || result.Id || result.ID;

        // Đóng modal thêm
        bootstrap.Modal.getInstance(document.getElementById("addLessonModal")).hide();

        // ==================== XỬ LÝ THEO LOẠI ====================
        if (type === 1) {
            // Reading → mở modal nhập nội dung
            if (typeof window.openReadingModal === "function") {
                window.openReadingModal(
                    currentChapterId,
                    newLessonId,
                    title,
                    "",           // content rỗng
                    isFree,
                    true          // isNew = true
                );
            }
        } else if (type === 2) {
            // Video → thông báo chức năng đang làm sau
            Toast.show("Chức năng thêm Video sẽ làm sau!", "info", 3000);
        } else if (type === 3) {
            // Quiz → mở modal Quiz mới
            if (typeof window.openQuizModal === "function") {
                window.openQuizModal(
                    currentChapterId,
                    newLessonId,
                    title,
                    null,   // quiz rỗng
                    true    // isNew = true
                );
            } else {
                Toast.show("Chưa load modal Quiz!", "danger", 3000);
            }
        }

        // ==================== CẬP NHẬT DANH SÁCH BÀI HỌC ====================
        const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
        window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);

        // ==================== THÔNG BÁO TẠO THÀNH CÔNG ====================
        Toast.show("Tạo bài học thành công!", "success", 3000);

    } catch (err) {
        console.error("Tạo bài học thất bại:", err);
        Toast.show("Tạo bài học thất bại!", "danger", 5000);
    }
}

export { };
