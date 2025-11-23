import { LessonType, appendLessonToDOM } from "./lesson.list.js";
import LessonApi from "../../api/lessonApi.js";
import Toast from "../../components/Toast.js";

let currentChapterId = null;

// ==============================
// Khởi tạo modal thêm bài học
// ==============================
export function initLessonInsert() {
    const chapterListContainer = document.getElementById("chapterList");
    chapterListContainer.addEventListener("click", (e) => {
        const btn = e.target.closest(".lesson-insert-btn");
        if (!btn) return;

        const chapterCard = btn.closest(".chapter-card");
        if (!chapterCard) return;

        currentChapterId = chapterCard.dataset.chapterId;

        document.getElementById("lessonName").value = "";
        document.getElementById("previewCheck").checked = false;

        const lessonTypeSelect = document.getElementById("lessonType");
        lessonTypeSelect.innerHTML = "";
        Object.entries(LessonType).forEach(([key, value]) => {
            const option = document.createElement("option");
            option.value = value;
            option.textContent = key;
            lessonTypeSelect.appendChild(option);
        });
    });
}

// ==============================
// Lưu bài học
// ==============================
async function saveLesson() {
    if (!currentChapterId) return;

    const name = document.getElementById("lessonName").value.trim();
    const type = parseInt(document.getElementById("lessonType").value);
    const isFree = document.getElementById("previewCheck").checked;

    if (!name) {
        Toast.show("Tên bài học không được để trống", "warning");
        return;
    }

    try {
        const newLesson = await LessonApi.createLesson(currentChapterId, { title: name, type, isFree });

        Toast.show("Tạo bài học thành công", "success");

        // Thêm bài học trực tiếp vào DOM
        appendLessonToDOM(currentChapterId, newLesson);

        // Tắt modal
        const modalEl = document.getElementById("addLessonModal");
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();

    } catch (err) {
        console.error(err);
        Toast.show("Tạo bài học thất bại", "danger");
    }
}

// ==============================
// DOM sẵn sàng
// ==============================
document.addEventListener("DOMContentLoaded", () => {
    initLessonInsert();
    document.getElementById("saveLessonBtn").addEventListener("click", saveLesson);
});
