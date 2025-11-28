// wwwroot/js/modules/instructor/lesson/lesson.updateContent.js

import LessonApi from "/js/api/lessonApi.js";
import Toast from "/js/components/Toast.js";

window.currentLessonEdit = null;
let quillInstance = null; // Chỉ khởi tạo 1 lần duy nhất

// === KHỞI TẠO QUILL CHỈ 1 LẦN DUY NHẤT ===
function initQuill() {
    if (quillInstance) return quillInstance;

    const container = document.getElementById("readingEditor");
    if (!container) return null;

    container.innerHTML = "";

    quillInstance = new Quill(container, {
        theme: "snow",
        modules: {
            toolbar: [
                [{ header: [1, 2, 3, false] }],
                ['bold', 'italic', 'underline', 'strike'],
                ['blockquote', 'code-block'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                [{ align: [] }],
                ['link', 'image', 'video'],
                ['clean']
            ]
        }
        placeholder: "Nhập nội dung bài học tại đây..."
    });

    return quillInstance;
}

// === MỞ MODAL BÀI ĐỌC (dùng cho cả TẠO MỚI và SỬA) ===
window.openReadingModal = (chapterId, lessonId, title = "", content = "", isFree = false, isNew = false) => {
    window.currentLessonEdit = {
        chapterId: parseInt(chapterId),
        lessonId: parseInt(lessonId),
        isNew
    };

    // Tiêu đề modal
    document.querySelector("#editReadingModal .modal-title").textContent =
        isNew ? "Tạo bài đọc mới" : "Chỉnh sửa bài đọc";

    // Điền dữ liệu
    document.getElementById("readingTitle").value = title || "";
    document.getElementById("readingFree").checked = isFree;

    // Khởi tạo Quill (chỉ 1 lần)
    const editor = initQuill();

    // Set nội dung (chờ 1 chút để Quill render xong)
    setTimeout(() => {
        if (editor) {
            editor.root.innerHTML = content || "";
        }
    }, 50);

    // Mở modal
    const modalEl = document.getElementById("editReadingModal");
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();

    // Khi modal đóng → làm sạch để lần sau mở lại không bị lỗi
    modalEl.addEventListener("hidden.bs.modal", () => {
        if (editor) editor.setContents([]);
        document.getElementById("readingTitle").value = "";
        document.getElementById("readingFree").checked = false;
    }, { once: true });
};

// === LƯU BÀI ĐỌC ===
window.saveReading = async () => {
    if (!window.currentLessonEdit) return;

    const title = document.getElementById("readingTitle")?.value.trim();
    const isFree = document.getElementById("readingFree")?.checked ?? false;
    const content = quillInstance ? quillInstance.root.innerHTML : "";

    if (!title || !content || content === "<p><br></p>") {
        Toast.show("Vui lòng nhập đầy đủ tên và nội dung!", "danger");
        return;
    }

    try {
        await LessonApi.updateLessonContent(
            window.currentLessonEdit.chapterId,
            window.currentLessonEdit.lessonId,
            { title, content, isFree }
        );

        bootstrap.Modal.getInstance(document.getElementById("editReadingModal")).hide();
        Toast.show("Lưu bài đọc thành công!", "success");

        // Reload danh sách
        const lessons = await LessonApi.getLessonsByChapter(window.currentLessonEdit.chapterId);
        window.lessonListModule.renderLessonsIntoChapter(window.currentLessonEdit.chapterId, lessons);

        window.currentLessonEdit = null;
    } catch (err) {
        Toast.show("Lưu thất bại!", "danger");
    }
};

// === HỖ TRỢ SỬA BÀI HỌC TỪ NÚT BÚT CHÌ (lesson.list.js gọi hàm này) ===
window.editLessonContent = async (chapterId, lessonId) => {
    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);
        if (lesson.type !== 1) return;

        window.openReadingModal(
            chapterId,
            lessonId,
            lesson.title,
            lesson.content || "",
            lesson.isFree,
            false // isNew = false → đang sửa
        );
    } catch (err) {
        Toast.show("Không tải được bài học!", "danger");
    }
};