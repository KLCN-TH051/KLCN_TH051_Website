// wwwroot/js/modules/instructor/lesson/lesson.list.js
import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";

const lessonTypeInfo = {
    1: { icon: "bi-journal-text", color: "text-primary", text: "Bài đọc" },
    2: { icon: "bi-play-circle", color: "text-success", text: "Video" },
    3: { icon: "bi-question-circle", color: "text-warning", text: "Quiz" }
};

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
                    <button class="btn btn-sm btn-outline-primary" title="Sửa"
                            onclick="window.editLesson(${chapterId}, ${lesson.id}, ${lesson.type})">
                        <i class="bi bi-pencil-square"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" title="Xóa"
                            onclick="window.deleteLesson(${chapterId}, ${lesson.id})">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        `;
    }).join("");

    initLessonSortable(container);
}

// Sortable cho bài học
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

// ==================== SỬA BÀI HỌC ====================
window.editLesson = (chapterId, lessonId, type) => {
    window.currentEditing = { chapterId, lessonId, type };
    // Mở modal tương ứng
    if (type === 1) new bootstrap.Modal(document.getElementById('editReadingModal')).show();
    if (type === 2) new bootstrap.Modal(document.getElementById('editVideoModal')).show();
    if (type === 3) new bootstrap.Modal(document.getElementById('editQuizModal')).show();
};

// ==================== XÓA BÀI HỌC VỚI MODAL XÁC NHẬN ====================
// Biến tạm để lưu thông tin bài học cần xóa
let lessonToDelete = null;

// Khi nhấn nút thùng rác → lưu dữ liệu + mở modal có sẵn
window.deleteLesson = (chapterId, lessonId) => {
    lessonToDelete = { chapterId, lessonId };
    // Dùng modal bạn đã có sẵn
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
};

// Xử lý khi người dùng nhấn nút "Xóa" (id="btnConfirmDelete") trong modal của bạn
document.getElementById("btnConfirmDelete")?.addEventListener("click", async () => {
    if (!lessonToDelete) return;

    const { chapterId, lessonId } = lessonToDelete;

    try {
        await LessonApi.deleteLesson(chapterId, lessonId);

        // Đóng modal (dùng modal bạn đang có)
        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();

        // Reload lại danh sách bài học trong đúng chương đó
        const lessons = await LessonApi.getLessonsByChapter(chapterId);
        window.lessonListModule.renderLessonsIntoChapter(chapterId, lessons);

        // Dùng Toast đẹp thay cho alert
        Toast.show("Xóa bài học thành công!", "success", 3000);

    } catch (err) {
        console.error(err);
        Toast.show("Xóa bài học thất bại!", "danger", 4000);
    } finally {
        lessonToDelete = null;
    }
});

// Export để chapter.loadlist.js gọi được
window.lessonListModule = { renderLessonsIntoChapter };