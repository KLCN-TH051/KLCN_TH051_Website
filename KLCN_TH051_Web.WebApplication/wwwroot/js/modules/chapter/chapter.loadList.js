import ChapterApi from "../../api/chapterApi.js";
import Toast from "../../components/Toast.js";

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
                            <i class="bi bi-plus"></i> Thêm bài học
                        </button>

                        <!-- Giữ khung danh sách bài học nhưng không load -->
                        <p id="lesson-list-${chapter.id}" class="text-muted">
                            Chưa tải bài học.
                        </p>
                    </div>
                </div>
            `;

            chapterListContainer.appendChild(card);
        });

    } catch (err) {
        console.error(err);
        Toast.show("Lỗi khi tải danh sách chương!", "danger");
    }
}

document.addEventListener("DOMContentLoaded", loadChapterList);
