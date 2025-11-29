//// wwwroot/js/modules/instructor/lesson/lesson.list.js
//import LessonApi from "../../../api/lessonApi.js";
//import Toast from "../../../components/Toast.js";

//const lessonTypeInfo = {
//    1: { icon: "bi-journal-text", color: "text-primary", text: "Bài đọc" },
//    2: { icon: "bi-play-circle", color: "text-success", text: "Video" },
//    3: { icon: "bi-question-circle", color: "text-warning", text: "Quiz" }
//};

//// ==================== RENDER DANH SÁCH BÀI HỌC ====================
//export function renderLessonsIntoChapter(chapterId, lessons = []) {
//    const container = document.querySelector(`[data-chapter-id="${chapterId}"] .lesson-list`);
//    if (!container) return;

//    if (!lessons || lessons.length === 0) {
//        container.innerHTML = `<p class="text-muted fst-italic">Chưa có bài học nào</p>`;
//        return;
//    }

//    container.innerHTML = lessons.map(lesson => {
//        const info = lessonTypeInfo[lesson.type] || lessonTypeInfo[1];
//        const duration = lesson.durationMinutes
//            ? `${Math.floor(lesson.durationMinutes / 60)}:${String(lesson.durationMinutes % 60).padStart(2, '0')}`
//            : '';
//        const quizInfo = lesson.type === 3 ? ` - ${lesson.quiz?.questions?.length || 0} câu hỏi` : '';

//        return `
//            <div class="lesson-item p-3 mb-2 d-flex justify-content-between align-items-center bg-white border rounded"
//                 data-lesson-id="${lesson.id}">
//                <div class="d-flex align-items-center flex-grow-1">
//                    <i class="bi bi-grip-vertical me-3 handle text-muted cursor-move"></i>
//                    <i class="bi ${info.icon} me-2 ${info.color}"></i>
//                    <div class="flex-grow-1">
//                        <div class="d-flex align-items-center gap-2 flex-wrap">
//                            <strong class="text-dark">${escapeHtml(lesson.title)}</strong>
//                            ${lesson.isFree ? '<span class="badge bg-success text-white px-2 py-1" style="font-size: 0.75rem;">Miễn phí</span>' : ''}
//                        </div>
//                        <small class="d-block text-muted mt-1">
//                            ${info.text}${duration ? ' · ' + duration : ''}${quizInfo}
//                        </small>
//                    </div>
//                </div>
//                <div class="d-flex gap-1">
//                    <button class="btn btn-sm btn-outline-primary edit-lesson-btn"
//                            data-chapter-id="${chapterId}"
//                            data-lesson-id="${lesson.id}"
//                            data-type="${lesson.type}"
//                            title="Sửa nội dung">
//                        <i class="bi bi-pencil-square"></i>
//                    </button>
//                    <button class="btn btn-sm btn-outline-danger"
//                            onclick="window.deleteLesson(${chapterId}, ${lesson.id})"
//                            title="Xóa">
//                        <i class="bi bi-trash"></i>
//                    </button>
//                </div>
//            </div>
//        `;
//    }).join("");

//    initLessonSortable(container);
//}

//// ==================== INIT SORTABLE ====================
//function initLessonSortable(container) {
//    if (container.sortableInitialized) return;
//    new Sortable(container, {
//        group: 'lessons',
//        handle: '.handle',
//        animation: 150,
//        ghostClass: 'bg-light'
//    });
//    container.sortableInitialized = true;
//}

//// ==================== ESCAPE HTML ====================
//function escapeHtml(text) {
//    const div = document.createElement('div');
//    div.textContent = text || '';
//    return div.innerHTML;
//}

//// ==================== XÓA BÀI HỌC ====================
//let lessonToDelete = null;
//window.deleteLesson = (chapterId, lessonId) => {
//    lessonToDelete = { chapterId, lessonId };
//    new bootstrap.Modal(document.getElementById('deleteModal')).show();
//};

//document.getElementById("btnConfirmDelete")?.addEventListener("click", async () => {
//    if (!lessonToDelete) return;
//    const { chapterId, lessonId } = lessonToDelete;
//    try {
//        await LessonApi.deleteLesson(chapterId, lessonId);
//        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
//        const lessons = await LessonApi.getLessonsByChapter(chapterId);
//        window.lessonListModule.renderLessonsIntoChapter(chapterId, lessons);
//        Toast.show("Xóa bài học thành công!", "success", 3000);
//    } catch (err) {
//        console.error(err);
//        Toast.show("Xóa bài học thất bại!", "danger", 4000);
//    } finally {
//        lessonToDelete = null;
//    }
//});

//// ==================== SỬA BÀI HỌC ====================
//window.editLesson = async (chapterId, lessonId, type) => {
//    try {
//        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

//        if (type === 1) {
//            document.getElementById("editReadingTitle").value = lesson.title || "";
//            document.getElementById("editReadingContent").value = lesson.content || "";
//            document.getElementById("editReadingFree").checked = lesson.isFree;
//            new bootstrap.Modal(document.getElementById('editReadingModal')).show();
//        } else if (type === 2) {
//            document.getElementById("editVideoTitle").value = lesson.title || "";
//            document.getElementById("editVideoUrl").value = lesson.videoUrl || "";
//            document.getElementById("editVideoDuration").value = lesson.durationMinutes || 0;
//            document.getElementById("editVideoFree").checked = lesson.isFree;
//            new bootstrap.Modal(document.getElementById('editVideoModal')).show();
//        } else if (type === 3) {
//            new bootstrap.Modal(document.getElementById('editQuizModal')).show();
//        }
//    } catch (err) {
//        console.error(err);
//        Toast.show("Không tải được thông tin bài học!", "danger");
//    }
//};

//// ==================== CLICK SỬA BUTTON ====================
//document.addEventListener("click", async (e) => {
//    const btn = e.target.closest(".edit-lesson-btn");
//    if (!btn) return;

//    const chapterId = parseInt(btn.dataset.chapterId);
//    const lessonId = parseInt(btn.dataset.lessonId);
//    const type = parseInt(btn.dataset.type);

//    if (type === 1) {
//        // Bài đọc
//        if (typeof window.openReadingModal === "function") {
//            const lesson = await LessonApi.getLessonById(chapterId, lessonId);
//            window.openReadingModal(
//                chapterId,
//                lessonId,
//                lesson.title || "",
//                lesson.content || "",
//                lesson.isFree || false,
//                false
//            );
//        } else {
//            Toast.show("Chưa load trình soạn thảo!", "danger");
//        }
//    } else if (type === 2) {
//        // Video
//        if (typeof window.openVideoModal === "function") {
//            window.openVideoModal(chapterId, lessonId);
//        } else {
//            Toast.show("Modal video chưa được load!", "danger");
//        }
//    } else if (type === 3) {
//        // Quiz
//        if (typeof window.editQuiz === "function") {
//            window.editQuiz(lessonId);
//        } else {
//            Toast.show("Chưa load Quiz editor!", "danger");
//        }
//    } else {
//        Toast.show("Loại bài học không hỗ trợ sửa!", "info");
//    }
//});

//// ==================== TẠO BÀI HỌC MỚI ====================
//document.getElementById("saveLessonBtn")?.addEventListener("click", async () => {
//    const lessonName = document.getElementById("lessonName").value.trim();
//    const lessonType = parseInt(document.getElementById("lessonType").value);
//    const isFree = document.getElementById("previewCheck").checked;

//    if (!lessonName || !lessonType) {
//        Toast.show("Vui lòng nhập tên và loại bài học!", "warning");
//        return;
//    }

//    try {
//        const chapterId = parseInt(document.getElementById("addLessonModal").dataset.chapterId || 1);

//        // Tạo bài học mới
//        const newLesson = await LessonApi.createLesson(chapterId, {
//            title: lessonName,
//            type: lessonType,
//            isFree
//        });

//        // Load lại danh sách bài học
//        const lessons = await LessonApi.getLessonsByChapter(chapterId);
//        window.lessonListModule.renderLessonsIntoChapter(chapterId, lessons);

//        // Nếu type là Video → mở modal editVideoModal
//        if (lessonType === 2 && typeof window.openVideoModal === "function") {
//            window.openVideoModal(chapterId, newLesson.id);
//        }

//        bootstrap.Modal.getInstance(document.getElementById("addLessonModal")).hide();
//        Toast.show("Tạo bài học thành công!", "success");
//    } catch (err) {
//        console.error(err);
//        Toast.show("Tạo bài học thất bại!", "danger");
//    }
//});

//// ==================== EXPORT MODULE ====================
//window.lessonListModule = { renderLessonsIntoChapter };

// wwwroot/js/modules/instructor/lesson/lesson.list.js
import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";

const lessonTypeInfo = {
    1: { icon: "bi-journal-text", color: "text-primary", text: "Bài đọc" },
    2: { icon: "bi-play-circle", color: "text-success", text: "Video" },
    3: { icon: "bi-question-circle", color: "text-warning", text: "Quiz" }
};

// ==================== RENDER DANH SÁCH BÀI HỌC ====================
export function renderLessonsIntoChapter(chapterId, lessons = []) {
    const container = document.querySelector(`[data-chapter-id="${chapterId}"] .lesson-list`);
    if (!container) return;

    if (!lessons || lessons.length === 0) {
        container.innerHTML = `<p class="text-muted fst-italic">Chưa có bài học nào</p>`;
        return;
    }

    container.innerHTML = lessons.map(lesson => {
        const info = lessonTypeInfo[lesson.type] || lessonTypeInfo[1];
        const duration = lesson.durationMinutes
            ? `${Math.floor(lesson.durationMinutes / 60)}:${String(lesson.durationMinutes % 60).padStart(2, '0')}`
            : '';
        const quizInfo = lesson.type === 3 ? ` - ${lesson.quiz?.questions?.length || 0} câu hỏi` : '';

        return `
            <div class="lesson-item p-3 mb-2 d-flex justify-content-between align-items-center bg-white border rounded"
                 data-lesson-id="${lesson.id}">
                <div class="d-flex align-items-center flex-grow-1">
                    <i class="bi bi-grip-vertical me-3 handle text-muted cursor-move"></i>
                    <i class="bi ${info.icon} me-2 ${info.color}"></i>
                    <div class="flex-grow-1">
                        <div class="d-flex align-items-center gap-2 flex-wrap">
                            <strong class="text-dark">${escapeHtml(lesson.title)}</strong>
                            ${lesson.isFree ? '<span class="badge bg-success text-white px-2 py-1" style="font-size: 0.75rem;">Miễn phí</span>' : ''}
                        </div>
                        <small class="d-block text-muted mt-1">
                            ${info.text}${duration ? ' · ' + duration : ''}${quizInfo}
                        </small>
                    </div>
                </div>
                <div class="d-flex gap-1">
                    <button class="btn btn-sm btn-outline-primary edit-lesson-btn"
                            data-chapter-id="${chapterId}"
                            data-lesson-id="${lesson.id}"
                            data-type="${lesson.type}"
                            title="Sửa nội dung">
                        <i class="bi bi-pencil-square"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger delete-lesson-btn"
                            data-chapter-id="${chapterId}"
                            data-lesson-id="${lesson.id}"
                            title="Xóa">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        `;
    }).join("");

    initLessonSortable(container);
}

// ==================== INIT SORTABLE ====================
function initLessonSortable(container) {
    if (container.sortableInitialized) return;
    new Sortable(container, {
        group: 'lessons',
        handle: '.handle',
        animation: 150,
        ghostClass: 'bg-light'
    });
    container.sortableInitialized = true;
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text || '';
    return div.innerHTML;
}

// ==================== XÓA BÀI HỌC ====================
let lessonToDelete = null;

window.deleteLesson = (chapterId, lessonId) => {
    lessonToDelete = { chapterId, lessonId };
    bootstrap.Modal.getOrCreateInstance(document.getElementById('deleteModal')).show();
};

// Xác nhận xóa
document.getElementById("btnConfirmDelete")?.addEventListener("click", async () => {
    if (!lessonToDelete) return;
    const { chapterId, lessonId } = lessonToDelete;
    try {
        await LessonApi.deleteLesson(chapterId, lessonId);
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();
        const lessons = await LessonApi.getLessonsByChapter(chapterId);
        window.lessonListModule.renderLessonsIntoChapter(chapterId, lessons);
        Toast.show("Xóa bài học thành công!", "success", 3000);
    } catch (err) {
        Toast.show("Xóa bài học thất bại!", "danger", 4000);
    } finally {
        lessonToDelete = null;
    }
});

// ==================== SỬA BÀI HỌC (chỉ xử lý nút bút chì) ====================
document.body.addEventListener("click", async (e) => {
    const btn = e.target.closest(".edit-lesson-btn");
    if (!btn) return;

    const chapterId = btn.dataset.chapterId;
    const lessonId = btn.dataset.lessonId;
    const type = parseInt(btn.dataset.type);

    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

        if (type === 1 && typeof window.openReadingModal === "function") {
            window.openReadingModal(chapterId, lessonId, lesson.title, lesson.content || "", lesson.isFree, false);
        } else if (type === 2 && typeof window.openVideoModal === "function") {
            window.openVideoModal(chapterId, lessonId);
        } else if (type === 3 && typeof window.editQuiz === "function") {
            window.editQuiz(lessonId);
        } else {
            Toast.show("Chưa hỗ trợ chỉnh sửa loại này!", "info");
        }
    } catch (err) {
        Toast.show("Không tải được bài học!", "danger");
    }
});

// XÓA HẾT CÁI CLICK SAVELESSONBTN TRÙNG LẶP Ở ĐÂY (đã có ở insert.js)
// → XÓA HOÀN TOÀN đoạn code từ dòng 100 trở xuống trong file cũ của bạn

// ==================== EXPORT MODULE ====================
window.lessonListModule = { renderLessonsIntoChapter };
