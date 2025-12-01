import ChapterApi from "../../../api/chapterApi.js";
import LessonApi from "../../../api/lessonApi.js";

document.addEventListener("DOMContentLoaded", async () => {
    const courseId = window.currentCourseId;
    const currentLessonId = window.currentLessonId;

    if (!courseId) return console.error("Không tìm thấy courseId từ view.");

    const sidebar = document.querySelector(".col-3 .list-group");
    const mainContent = document.getElementById("mainContent");

    sidebar.innerHTML = "<li class='list-group-item'>Đang tải chapters...</li>";
    mainContent.innerHTML = "<p>Đang tải nội dung bài học...</p>";

    try {
        // 1️⃣ Lấy danh sách chapter theo course
        const chapters = await ChapterApi.getByCourse(courseId);
        if (!chapters.length) {
            sidebar.innerHTML = "<li class='list-group-item'>Chưa có chapter nào.</li>";
            mainContent.innerHTML = "<p>Chưa có bài học nào.</p>";
            return;
        }

        sidebar.innerHTML = "";

        for (let cIndex = 0; cIndex < chapters.length; cIndex++) {
            const chapter = chapters[cIndex];
            const chapterName = chapter.title || chapter.name || `Chapter ${cIndex + 1}`;

            const liChapter = document.createElement("li");
            liChapter.className = "list-group-item";

            const chapterTitle = document.createElement("strong");
            chapterTitle.textContent = `Chương ${cIndex + 1}: ${chapterName}`;
            liChapter.appendChild(chapterTitle);

            // List lesson trong chapter
            const ulLesson = document.createElement("ul");
            ulLesson.className = "list-group list-group-flush mt-2";

            // Lấy lesson theo chapter
            const lessons = await LessonApi.getLessonsByChapter(chapter.id);

            lessons.forEach((lesson, lIndex) => {
                const liLesson = document.createElement("li");
                liLesson.className = "list-group-item lesson-item";
                liLesson.style.cursor = "pointer";

                // Tên bài học
                const lessonName = lesson.title || lesson.name || `Bài học ${lIndex + 1}`;
                liLesson.innerHTML = `<strong>${lessonName}</strong><br>`;

                // Loại bài học
                let typeText = "";
                switch (lesson.type) {
                    case 1: typeText = "Bài đọc"; break;
                    case 2: typeText = "Video"; break;
                    case 3: typeText = "Quiz"; break;
                    default: typeText = "Bài học";
                }
                const typeSpan = document.createElement("span");
                typeSpan.className = "text-muted";
                typeSpan.textContent = typeText;
                liLesson.appendChild(typeSpan);

                // Nếu isFree
                if (lesson.isFree) {
                    const badge = document.createElement("span");
                    badge.className = "badge bg-success ms-2";
                    badge.textContent = "Xem trước";
                    liLesson.appendChild(badge);
                }

                // Click event
                liLesson.addEventListener("click", async () => {
                    if (lesson.isFree) {
                        try {
                            const lessonData = await LessonApi.getLessonById(chapter.id, lesson.id);
                            mainContent.innerHTML = `
                                <h5>${lessonName}</h5>
                                <p>Loại: ${typeText}</p>
                                <div>${lessonData.content || "<em>Chưa có nội dung</em>"}</div>
                            `;
                        } catch (err) {
                            console.error(err);
                            mainContent.innerHTML = "<p class='text-danger'>Lỗi khi tải nội dung bài học.</p>";
                        }
                    } else {
                        alert("Khóa học này chưa được mở. Vui lòng thanh toán để xem bài học.");
                    }
                });

                ulLesson.appendChild(liLesson);
            });

            liChapter.appendChild(ulLesson);
            sidebar.appendChild(liChapter);
        }

        // Nếu có currentLessonId thì mở ngay bài học đó
        if (currentLessonId) {
            for (let li of document.querySelectorAll(".lesson-item")) {
                if (li.dataset.lessonId == currentLessonId) {
                    li.click();
                    break;
                }
            }
        }

    } catch (err) {
        console.error("Lỗi khi gọi API", err);
        sidebar.innerHTML = "<li class='list-group-item text-danger'>Lỗi khi tải chapter.</li>";
        mainContent.innerHTML = "<p class='text-danger'>Lỗi khi tải bài học.</p>";
    }
});
