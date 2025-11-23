import LessonApi from "../../api/lessonApi.js";
import Toast from "../../components/Toast.js";

// ==============================
// Enum kiểu số giống C#
// ==============================
export const LessonType = {
    Content: 1,
    Video: 2,
    Quiz: 3
};

// ==============================
// Cache lesson theo chapterId
// ==============================
const lessonCache = {};

// ==============================
// Load lesson cho từng chapter
// ==============================
export async function loadLessonList(chapterId) {
    const container = document.getElementById(`lesson-list-${chapterId}`);
    if (!container) return;

    // ==========================
    // Helper render nội dung
    // ==========================
    const render = (lessons) => {
        container.innerHTML = lessons.length
            ? lessons.map(renderLessonItem).join("")
            : `<p class="text-muted fst-italic">Chưa có bài học nào.</p>`;
    };

    // Nếu đã có cache → render ngay
    if (Array.isArray(lessonCache[chapterId])) {
        render(lessonCache[chapterId]);
        return lessonCache[chapterId];
    }

    container.innerHTML = `<p class="text-muted">Đang tải bài học...</p>`;

    try {
        const lessons = await LessonApi.getLessons(chapterId) || [];
        lessonCache[chapterId] = lessons;

        render(lessons);

        // ==========================
        // MutationObserver chống mất text
        // ==========================
        const observer = new MutationObserver(() => {
            if (!container.hasChildNodes() || container.textContent.trim() === "") {
                render(lessons);
            }
        });

        observer.observe(container, { childList: true, subtree: true });

        return lessons;
    } catch (err) {
        console.error(err);
        container.innerHTML = `<p class="text-danger">Không thể tải bài học.</p>`;
        lessonCache[chapterId] = [];
        return [];
    }
}

// ==============================
// Thêm bài học trực tiếp vào DOM
// ==============================
export function appendLessonToDOM(chapterId, lesson) {
    const container = document.getElementById(`lesson-list-${chapterId}`);
    if (!container) return;

    if (!Array.isArray(lessonCache[chapterId])) lessonCache[chapterId] = [];

    // Xoá text mặc định nếu chỉ có thông báo
    const defaultTexts = ["Chưa có bài học", "Đang tải"];
    if (defaultTexts.some(txt => container.textContent.includes(txt))) {
        container.innerHTML = "";
    }

    container.insertAdjacentHTML("beforeend", renderLessonItem(lesson));
    lessonCache[chapterId].push(lesson);
}

// ==============================
// Render từng lesson
// ==============================
export function renderLessonItem(lesson) {
    const iconClass = getLessonIcon(lesson.type);
    const typeText = getLessonTypeText(lesson);

    return `
    <div class="lesson-item p-3 mb-2 d-flex justify-content-between align-items-center">
        <div class="d-flex align-items-center">
            <i class="bi bi-grip-vertical me-3 handle"></i>
            <i class="${iconClass} me-2"></i>
            <div>
                <strong>${lesson.title}</strong>
                ${lesson.isFree ? '<span class="badge bg-success ms-2">Miễn phí</span>' : ''}
                <small class="d-block text-muted">${typeText}</small>
            </div>
        </div>
        <div>
            <button class="btn btn-sm btn-outline-primary me-1" onclick="editLesson(${lesson.type}, ${lesson.id})">
                <i class="bi bi-pencil-square"></i>
            </button>
            <button class="btn btn-sm btn-outline-danger" onclick="deleteLesson(${lesson.id})">
                <i class="bi bi-trash"></i>
            </button>
        </div>
    </div>
    `;
}

// ==============================
// Icon theo loại lesson
// ==============================
function getLessonIcon(type) {
    switch (type) {
        case LessonType.Content: return "bi bi-journal-text text-primary";
        case LessonType.Video: return "bi bi-play-circle text-success";
        case LessonType.Quiz: return "bi bi-question-circle text-warning";
        default: return "bi bi-file-earmark";
    }
}

// ==============================
// Text hiển thị loại lesson
// ==============================
function getLessonTypeText(lesson) {
    switch (lesson.type) {
        case LessonType.Content: return "Bài đọc";
        case LessonType.Video: return `Video - ${lesson.durationMinutes || 0} phút`;
        case LessonType.Quiz: return `Quiz - ${lesson.totalQuestions || 0} câu hỏi`;
        default: return "Không xác định";
    }
}

// ==============================
// Hàm global để edit/delete
// ==============================
window.editLesson = (type, id) => {
    console.log("Edit lesson", type, id);
    Toast.show("Chức năng chỉnh sửa sẽ được triển khai sau.", "info");
};

window.deleteLesson = async id => {
    if (!confirm("Bạn có chắc muốn xóa bài học này?")) return;

    try {
        await LessonApi.deleteLesson(id);
        Toast.show("Xóa bài học thành công", "success");

        const el = document.querySelector(`.lesson-item button[onclick*="deleteLesson(${id})"]`)?.closest(".lesson-item");
        if (el) el.remove();

        Object.keys(lessonCache).forEach(chapterId => {
            lessonCache[chapterId] = lessonCache[chapterId]?.filter(l => l.id !== id) || [];
        });

    } catch (err) {
        console.error(err);
        Toast.show("Xóa bài học thất bại", "danger");
    }
};
