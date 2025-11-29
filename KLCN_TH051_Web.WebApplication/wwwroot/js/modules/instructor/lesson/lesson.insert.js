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

    // Nút "Tạo bài học" trong modal → CHỈ GẮN 1 LẦN DUY NHẤT
    const saveBtn = document.getElementById("saveLessonBtn");
    if (saveBtn && !saveBtn.dataset.listenerAdded) {
        saveBtn.addEventListener("click", createLesson);
        saveBtn.dataset.listenerAdded = "true"; // đánh dấu đã gắn
    }
});

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
        const result = await LessonApi.createLesson(currentChapterId, {
            title,
            type,
            isFree
        });

        const newLessonId = result.id || result.Id || result.ID;

        // Đóng modal
        bootstrap.Modal.getInstance(document.getElementById("addLessonModal")).hide();

        // Mở modal tương ứng theo loại
        if (type === 1 && typeof window.openReadingModal === "function") {
            window.openReadingModal(currentChapterId, newLessonId, title, "", isFree, true);
        } else if (type === 2 && typeof window.openVideoModal === "function") {
            window.openVideoModal(currentChapterId, newLessonId);
        } else if (type === 3 && typeof window.openQuizModal === "function") {
            window.openQuizModal(currentChapterId, newLessonId, title, null, true);
        } else if (type === 2) {
            Toast.show("Chức năng Video đang phát triển!", "info");
        }

        // Cập nhật lại danh sách bài học
        if (typeof window.lessonListModule?.renderLessonsIntoChapter === "function") {
            const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
            window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);
        }

        Toast.show("Tạo bài học thành công!", "success", 3000);
    } catch (err) {
        console.error("Tạo bài học thất bại:", err);
        Toast.show("Tạo bài học thất bại! " + (err.message || ""), "danger", 5000);
    }
}

export { };

//// wwwroot/js/modules/instructor/lesson/lesson.insert.js
//import LessonApi from "/js/api/lessonApi.js";
//import Toast from "/js/components/Toast.js";

//let currentChapterId = null;
//let lessonTypesLoaded = false;
//let addLessonModalInstance = null;
//let isAddLessonOpening = false; // ⭐ lock thao tác mở modal nhanh

//// ==================== LOAD LESSON TYPES ====================
//async function loadLessonTypes() {
//    if (lessonTypesLoaded) return;
//    const select = document.getElementById("lessonType");
//    if (!select) return;

//    try {
//        const types = await LessonApi.getLessonTypes();
//        select.innerHTML = '<option value="" disabled selected>Chọn loại bài học</option>';
//        types.forEach(type => {
//            const option = document.createElement("option");
//            option.value = type.value;
//            option.textContent = type.label;
//            select.appendChild(option);
//        });
//        lessonTypesLoaded = true;
//    } catch (err) {
//        console.error("Lỗi load loại bài học:", err);
//        select.innerHTML = '<option value="">Lỗi tải dữ liệu</option>';
//        Toast.show("Không tải được loại bài học!", "danger");
//    }
//}

//// ==================== INIT ====================
//document.addEventListener("DOMContentLoaded", () => {
//    const modalEl = document.getElementById("addLessonModal");
//    if (!modalEl) return;

//    addLessonModalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);

//    // Khi modal đóng → reset lock và form
//    modalEl.addEventListener("hidden.bs.modal", () => {
//        isAddLessonOpening = false;
//        document.getElementById("lessonName").value = "";
//        const select = document.getElementById("lessonType");
//        if (select) select.selectedIndex = 0;
//        const previewCheck = document.getElementById("previewCheck");
//        if (previewCheck) previewCheck.checked = false;
//    });

//    // Mở modal thêm bài học
//    document.body.addEventListener("click", async (e) => {
//        const btn = e.target.closest(".lesson-insert-btn");
//        if (!btn) return;

//        if (isAddLessonOpening) return; // đang mở modal, ignore
//        isAddLessonOpening = true;

//        currentChapterId = btn.dataset.chapterId;
//        await loadLessonTypes();

//        addLessonModalInstance.show();
//    });

//    // Gắn listener cho nút save 1 lần duy nhất
//    const saveBtn = document.getElementById("saveLessonBtn");
//    if (saveBtn && !saveBtn.dataset.listenerAdded) {
//        saveBtn.addEventListener("click", createLesson);
//        saveBtn.dataset.listenerAdded = "true";
//    }
//});

//// ==================== CREATE LESSON ====================
//async function createLesson() {
//    const title = document.getElementById("lessonName")?.value.trim();
//    const typeValue = document.getElementById("lessonType")?.value;
//    const isFree = document.getElementById("previewCheck")?.checked ?? false;

//    if (!title || !typeValue) {
//        Toast.show("Vui lòng nhập tên và chọn loại bài học!", "danger", 4000);
//        return;
//    }

//    const type = parseInt(typeValue);

//    try {
//        const result = await LessonApi.createLesson(currentChapterId, {
//            title,
//            type,
//            isFree
//        });

//        const newLessonId = result.id || result.Id || result.ID;

//        // Đóng modal Add Lesson trước
//        addLessonModalInstance.hide();

//        // Sau khi modal đóng xong mới mở modal chi tiết
//        addLessonModalInstance._element.addEventListener(
//            "hidden.bs.modal",
//            async () => {
//                try {
//                    if (type === 1 && typeof window.openReadingModal === "function") {
//                        window.openReadingModal(currentChapterId, newLessonId, title, "", isFree, true);
//                    } else if (type === 2 && typeof window.openVideoModal === "function") {
//                        window.openVideoModal(currentChapterId, newLessonId);
//                    } else if (type === 3 && typeof window.openQuizModal === "function") {
//                        window.openQuizModal(currentChapterId, newLessonId, title, null, true);
//                    }

//                    // Refresh danh sách bài học
//                    if (typeof window.lessonListModule?.renderLessonsIntoChapter === "function") {
//                        const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
//                        window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);
//                    }
//                } catch (err) {
//                    console.error("Lỗi khi mở modal chi tiết:", err);
//                }
//            },
//            { once: true } // ⭐ chỉ chạy 1 lần
//        );

//        Toast.show("Tạo bài học thành công!", "success", 3000);

//    } catch (err) {
//        console.error("Tạo bài học thất bại:", err);
//        Toast.show("Tạo bài học thất bại! " + (err.message || ""), "danger", 5000);
//    }
//}

//export { };
