import ChapterApi from "../../../api/chapterApi.js";
import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";

const courseId = window.location.pathname.split("/").pop();
const chapterListContainer = document.getElementById("chapterList");

// Không còn cache lesson
// Không import loadLessonList, renderLessonItem

export async function loadChapterList() {
    if (!courseId) return;

    try {
        const chapters = await ChapterApi.getByCourse(courseId);
        chapterListContainer.innerHTML = "";

        if (!chapters || chapters.length === 0) {
            chapterListContainer.innerHTML = `<p class="text-muted">Chưa có chương nào.</p>`;
            return;
        }

        chapters.forEach(chapter => {
            const card = document.createElement("div");
            card.classList.add("chapter-card", "card", "mb-3");
            card.dataset.chapterId = chapter.id;

            card.innerHTML = `
                <div class="card-header d-flex justify-content-between align-items-center">
                    <div class="d-flex align-items-center">
                        <i class="bi bi-grip-vertical me-2 handle"></i>
                        <h6 class="mb-0 fw-bold">Chương ${chapter.order}: ${chapter.name}</h6>
                    </div>
                    <div>
                        <button class="btn btn-sm btn-outline-primary me-1" onclick="editChapter(${chapter.id})">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-outline-danger me-1" onclick="deleteChapter(${chapter.id})">
                            <i class="bi bi-trash"></i>
                        </button>
                        <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="collapse" data-bs-target="#chapterContent${chapter.id}">
                            <i class="bi bi-chevron-down"></i>
                        </button>
                    </div>
                </div>

                <div id="chapterContent${chapter.id}" class="collapse">
                    <div class="card-body">
                        <button class="btn btn-sm btn-success mb-3 lesson-insert-btn"
                            data-bs-toggle="modal"
                            data-bs-target="#addLessonModal"
                            data-chapter-id="${chapter.id}">
                            Thêm bài học
                            </button>

                        <!-- Container để lesson.list.js render vào -->
                        <div class="lesson-list" data-chapter-id="${chapter.id}">
                            <small class="text-muted">Đang tải bài học...</small>
                        </div>
                    </div>
                </div>
            `;

            chapterListContainer.appendChild(card);
            //======================================
            LessonApi.getLessonsByChapter(chapter.id)
                .then(lessons => {
                    const { renderLessonsIntoChapter } = window.lessonListModule || {};
                    if (renderLessonsIntoChapter) {
                        renderLessonsIntoChapter(chapter.id, lessons);
                    }
                })
                .catch(() => {
                    const container = card.querySelector('.lesson-list');
                    if (container) container.innerHTML = '<small class="text-danger">Lỗi tải bài học</small>';
                });
            //======================================
        });

    } catch (err) {
        console.error(err);
        Toast.show("Lỗi khi tải danh sách chương!", "danger");
    }
}

chapterListContainer.addEventListener('click', (e) => {
    const toggleBtn = e.target.closest('[data-bs-toggle="collapse"]');
    if (!toggleBtn) return;

    const icon = toggleBtn.querySelector('i');
    if (!icon) return;

    const target = document.querySelector(toggleBtn.dataset.bsTarget);
    if (!target) return;

    // Khi collapse đang mở → xoay xuống, đang đóng → xoay lên
    target.addEventListener('show.bs.collapse', () => {
        icon.classList.remove('bi-chevron-down');
        icon.classList.add('bi-chevron-up');
    });
    target.addEventListener('hide.bs.collapse', () => {
        icon.classList.remove('bi-chevron-up');
        icon.classList.add('bi-chevron-down');
    });
});

document.addEventListener("DOMContentLoaded", loadChapterList);
