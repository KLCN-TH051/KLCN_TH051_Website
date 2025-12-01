import ChapterApi from "../../../api/chapterApi.js";
import LessonApi from "../../../api/lessonApi.js";

document.addEventListener("DOMContentLoaded", async () => {
    const pathParts = window.location.pathname.split("/");
    const courseId = pathParts[pathParts.length - 1];

    if (!courseId) return console.error("Không tìm thấy courseId trong URL");

    const contentPane = document.getElementById("content-pane");
    contentPane.innerHTML = "<p>Đang tải nội dung khóa học...</p>";

    try {
        const chapters = await ChapterApi.getByCourse(courseId);
        if (!chapters.length) {
            contentPane.innerHTML = "<p>Chưa có chapter nào.</p>";
            return;
        }

        contentPane.innerHTML = "";
        const accordion = document.createElement("div");
        accordion.className = "accordion";
        accordion.id = "courseChaptersAccordion";

        chapters.forEach((chapter, index) => {
            const chapterId = `chapter-${chapter.id}`;
            const chapterTitle = chapter.title || chapter.name || `Chapter ${index + 1}`;

            const card = document.createElement("div");
            card.className = "accordion-item";

            card.innerHTML = `
                <h2 class="accordion-header" id="heading-${chapterId}">
                    <button class="accordion-button ${index !== 0 ? "collapsed" : ""}" type="button" data-bs-toggle="collapse" data-bs-target="#collapse-${chapterId}" aria-expanded="${index === 0}" aria-controls="collapse-${chapterId}">
                        Chapter ${index + 1}: ${chapterTitle}
                    </button>
                </h2>
                <div id="collapse-${chapterId}" class="accordion-collapse collapse ${index === 0 ? "show" : ""}" aria-labelledby="heading-${chapterId}" data-bs-parent="#courseChaptersAccordion">
                    <div class="accordion-body">
                        <p>Đang tải bài học...</p>
                    </div>
                </div>
            `;
            accordion.appendChild(card);

            // Load lesson khi mở collapse
            const collapseEl = card.querySelector(`#collapse-${chapterId}`);
            collapseEl.addEventListener("show.bs.collapse", async () => {
                const body = collapseEl.querySelector(".accordion-body");
                body.innerHTML = "<p>Đang tải bài học...</p>";
                try {
                    const lessons = await LessonApi.getLessonsByChapter(chapter.id);
                    if (!lessons.length) {
                        body.innerHTML = "<p>Chưa có bài học nào.</p>";
                        return;
                    }

                    const ul = document.createElement("ul");
                    ul.className = "list-group list-group-flush";

                    lessons.forEach((lesson, lIndex) => {
                        const li = document.createElement("li");
                        li.className = "list-group-item";
                        li.style.cursor = "pointer";

                        // Tên bài học chính
                        const lessonName = lesson.title || lesson.name || `Bài học ${lIndex + 1}`;
                        li.innerHTML = `<strong>${lessonName}</strong><br>`;

                        // Dòng mô tả loại bài học
                        let typeText = "";
                        switch (lesson.type) {
                            case 1: typeText = `Bài đọc`; break;
                            case 2: typeText = `Video`; break;
                            case 3: typeText = `Quiz`; break;
                            default: typeText = `Bài học`;
                        }
                        const typeSpan = document.createElement("span");
                        typeSpan.className = "text-muted";
                        typeSpan.textContent = typeText;
                        li.appendChild(typeSpan);

                        // Nếu isFree thì thêm tag "Xem trước"
                        if (lesson.isFree) {
                            const badge = document.createElement("span");
                            badge.className = "badge bg-success ms-2";
                            badge.textContent = "Xem trước";
                            li.appendChild(badge);
                        }

                        // Click hành vi
                        li.addEventListener("click", () => {
                            if (lesson.isFree) {
                                window.location.href = `/Course/Lessons/${lesson.id}`;
                            } else {
                                const paymentModal = new bootstrap.Modal(document.getElementById('paymentModal'));
                                paymentModal.show();
                            }
                        });

                        ul.appendChild(li);
                    });

                    body.innerHTML = "";
                    body.appendChild(ul);

                } catch (err) {
                    console.error(err);
                    body.innerHTML = "<p class='text-danger'>Lỗi khi tải bài học.</p>";
                }
            });
        });

        contentPane.appendChild(accordion);

    } catch (err) {
        console.error(err);
        contentPane.innerHTML = "<p class='text-danger'>Lỗi khi tải chapter.</p>";
    }
});
