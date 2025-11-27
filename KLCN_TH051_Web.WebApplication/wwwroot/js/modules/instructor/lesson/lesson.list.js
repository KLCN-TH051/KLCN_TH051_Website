//// wwwroot/js/modules/instructor/lesson/lesson.list.js
//import LessonApi from "../../../api/lessonApi.js";
//import Toast from "../../../components/Toast.js";

//const lessonTypeInfo = {
//    1: { icon: "bi-journal-text", color: "text-primary", text: "Bài đọc" },
//    2: { icon: "bi-play-circle", color: "text-success", text: "Video" },
//    3: { icon: "bi-question-circle", color: "text-warning", text: "Quiz" }
//};

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
//                    <button class="btn btn-sm btn-outline-primary" title="Sửa"
//                            onclick="window.editLesson(${chapterId}, ${lesson.id}, ${lesson.type})">
//                        <i class="bi bi-pencil-square"></i>
//                    </button>
//                    <button class="btn btn-sm btn-outline-danger" title="Xóa"
//                            onclick="window.deleteLesson(${chapterId}, ${lesson.id})">
//                        <i class="bi bi-trash"></i>
//                    </button>
//                </div>
//            </div>
//        `;
//    }).join("");

//    initLessonSortable(container);
//}

//// Sortable cho bài học
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

//function escapeHtml(text) {
//    const div = document.createElement('div');
//    div.textContent = text || '';
//    return div.innerHTML;
//}

////// ==================== SỬA BÀI HỌC ====================
////window.editLesson = async (chapterId, lessonId, type) => {
////    try {
////        // BƯỚC 1: Lấy chi tiết bài học từ server
////        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

////        // BƯỚC 2: Điền dữ liệu vào các modal tương ứng
////        if (type === 1) { // Bài đọc
////            document.getElementById("editReadingTitle").value = lesson.title || "";
////            document.getElementById("editReadingContent").value = lesson.content || "";
////            document.getElementById("editReadingFree").checked = lesson.isFree;
////            new bootstrap.Modal(document.getElementById('editReadingModal')).show();
////        }
////        else if (type === 2) { // Video
////            document.getElementById("editVideoTitle").value = lesson.title || "";
////            document.getElementById("editVideoUrl").value = lesson.videoUrl || "";
////            document.getElementById("editVideoDuration").value = lesson.durationMinutes || 0;
////            document.getElementById("editVideoFree").checked = lesson.isFree;
////            new bootstrap.Modal(document.getElementById('editVideoModal')).show();
////        }
////        else if (type === 3) {
////            // Quiz xử lý sau
////            new bootstrap.Modal(document.getElementById('editQuizModal')).show();
////        }
////    } catch (err) {
////        console.error(err);
////        Toast.show("Không tải được thông tin bài học!", "danger");
////    }
////};

//// ==================== XÓA BÀI HỌC VỚI MODAL XÁC NHẬN ====================
//// Biến tạm để lưu thông tin bài học cần xóa
//let lessonToDelete = null;

//// Khi nhấn nút thùng rác → lưu dữ liệu + mở modal có sẵn
//window.deleteLesson = (chapterId, lessonId) => {
//    lessonToDelete = { chapterId, lessonId };
//    // Dùng modal bạn đã có sẵn
//    new bootstrap.Modal(document.getElementById('deleteModal')).show();
//};

//// Xử lý khi người dùng nhấn nút "Xóa" (id="btnConfirmDelete") trong modal của bạn
//document.getElementById("btnConfirmDelete")?.addEventListener("click", async () => {
//    if (!lessonToDelete) return;

//    const { chapterId, lessonId } = lessonToDelete;

//    try {
//        await LessonApi.deleteLesson(chapterId, lessonId);

//        // Đóng modal (dùng modal bạn đang có)
//        bootstrap.Modal.getInstance(document.getElementById('deleteModal')).hide();

//        // Reload lại danh sách bài học trong đúng chương đó
//        const lessons = await LessonApi.getLessonsByChapter(chapterId);
//        window.lessonListModule.renderLessonsIntoChapter(chapterId, lessons);

//        // Dùng Toast đẹp thay cho alert
//        Toast.show("Xóa bài học thành công!", "success", 3000);

//    } catch (err) {
//        console.error(err);
//        Toast.show("Xóa bài học thất bại!", "danger", 4000);
//    } finally {
//        lessonToDelete = null;
//    }
//});

//// Export để chapter.loadlist.js gọi được
//window.lessonListModule = { renderLessonsIntoChapter };

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
                    <!-- NÚT SỬA: DÙNG CLASS + DATA ATTRIBUTE, KHÔNG DÙNG ONCLICK TRỰC TIẾP -->
                    <button class="btn btn-sm btn-outline-primary edit-lesson-btn"
                            data-chapter-id="${chapterId}"
                            data-lesson-id="${lesson.id}"
                            data-type="${lesson.type}"
                            title="Sửa nội dung">
                        <i class="bi bi-pencil-square"></i>
                    </button>
                    <!-- NÚT XÓA: GIỮ NGUYÊN ONCLICK VÌ ĐÃ HOẠT ĐỘNG TỐT -->
                    <button class="btn btn-sm btn-outline-danger"
                            onclick="window.deleteLesson(${chapterId}, ${lesson.id})"
                            title="Xóa">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        `;
    }).join("");

    initLessonSortable(container);
}

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

// ==================== XÓA BÀI HỌC (GIỮ NGUYÊN – ĐÃ HOÀN HẢO) ====================
let lessonToDelete = null;
window.deleteLesson = (chapterId, lessonId) => {
    lessonToDelete = { chapterId, lessonId };
    new bootstrap.Modal(document.getElementById('deleteModal')).show();
};

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
        console.error(err);
        Toast.show("Xóa bài học thất bại!", "danger", 4000);
    } finally {
        lessonToDelete = null;
    }
});

// ==================== SỬA BÀI HỌC – CHỈ DÀNH CHO BÀI ĐỌC (TYPE = 1) ====================
//document.addEventListener("click", async (e) => {
//    const btn = e.target.closest(".edit-lesson-btn");
//    if (!btn) return;

//    const chapterId = parseInt(btn.dataset.chapterId);
//    const lessonId = parseInt(btn.dataset.lessonId);
//    const type = parseInt(btn.dataset.type);

//    if (type !== 1 && type !== 3) {
//        Toast.show("Chỉ hỗ trợ sửa bài đọc hiện tại!", "info");
//        return;
//    }


//    try {
//        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

//        // GỌI HÀM TỪ lesson.updateContent.js ĐÃ CÓ SẴN
//        if (typeof window.openReadingModal === "function") {
//            window.openReadingModal(
//                chapterId,
//                lessonId,
//                lesson.title || "",
//                lesson.content || "",
//                lesson.isFree || false,
//                false // đang sửa, không phải tạo mới
//            );
//        } else {
//            Toast.show("Chưa load trình soạn thảo!", "danger");
//        }
//    } catch (err) {
//        console.error(err);
//        Toast.show("Không tải được bài học!", "danger");
//    }
//});

//// Export module
//window.lessonListModule = { renderLessonsIntoChapter };

document.addEventListener("click", async (e) => {
    const btn = e.target.closest(".edit-lesson-btn");
    if (!btn) return;

    const chapterId = parseInt(btn.dataset.chapterId);
    const lessonId = parseInt(btn.dataset.lessonId);
    const type = parseInt(btn.dataset.type);

    if (type === 1) {
        // Bài đọc
        if (typeof window.openReadingModal === "function") {
            const lesson = await LessonApi.getLessonById(chapterId, lessonId);
            window.openReadingModal(
                chapterId,
                lessonId,
                lesson.title || "",
                lesson.content || "",
                lesson.isFree || false,
                false
            );
        } else {
            Toast.show("Chưa load trình soạn thảo!", "danger");
        }
    } else if (type === 3) {
        // Quiz
        if (typeof window.openQuizModal === "function") {
            window.openQuizModal(
                chapterId,
                lessonId
            );
        } else {
            Toast.show("Chưa load Quiz editor!", "danger");
        }
    } else {
        Toast.show("Chỉ hỗ trợ sửa bài đọc và Quiz hiện tại!", "info");
    }
});
window.lessonListModule = { renderLessonsIntoChapter };

