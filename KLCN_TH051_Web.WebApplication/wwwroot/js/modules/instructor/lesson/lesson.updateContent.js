// wwwroot/js/modules/instructor/lesson/lesson.updateContent.js

import LessonApi from "/js/api/lessonApi.js";
import Toast from "/js/components/Toast.js";
import UploadApi from "/js/api/uploadApi.js"; // nếu muốn hỗ trợ upload ảnh vào Quill

window.currentLessonEdit = null;
let quillInstance = null; // Khởi tạo 1 lần duy nhất

// === KHỞI TẠO QUILL CHỈ 1 LẦN DUY NHẤT ===
function initQuill() {
    if (quillInstance) return quillInstance;

    const container = document.getElementById("readingEditor");
    if (!container) return null;

    container.innerHTML = "";

    quillInstance = new Quill(container, {
        theme: "snow",
        modules: {
            toolbar: {
                container: [
                    [{ header: [1, 2, 3, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    ['blockquote', 'code-block'],
                    [{ list: 'ordered' }, { list: 'bullet' }],
                    [{ align: [] }],
                    ['link', 'image', 'video'],
                    ['clean']
                ],
                handlers: {
                    // Tùy chọn nếu muốn xử lý upload ảnh trực tiếp
                    image: function () {
                        const input = document.createElement('input');
                        input.setAttribute('type', 'file');
                        input.setAttribute('accept', 'image/*');
                        input.click();

                        input.onchange = async () => {
                            const file = input.files[0];
                            if (file) {
                                try {
                                    const res = await UploadApi.uploadFile(file, 'content');
                                    const range = this.quill.getSelection();
                                    this.quill.insertEmbed(range.index, 'image', res.fileUrl);
                                } catch (err) {
                                    Toast.show("Upload ảnh thất bại!", "danger");
                                }
                            }
                        };
                    }
                }
            }
        },
        placeholder: "Nhập nội dung bài học tại đây..."
    });

    return quillInstance;
}

// === MỞ MODAL BÀI ĐỌC (TẠO MỚI hoặc SỬA) ===
window.openReadingModal = (chapterId, lessonId, title = "", content = "", isFree = false, isNew = false) => {
    window.currentLessonEdit = {
        chapterId: parseInt(chapterId),
        lessonId: parseInt(lessonId),
        isNew
    };

    const modalTitleEl = document.querySelector("#editReadingModal .modal-title");
    if (modalTitleEl) modalTitleEl.textContent = isNew ? "Tạo bài đọc mới" : "Chỉnh sửa bài đọc";

    const titleEl = document.getElementById("readingTitle");
    const freeEl = document.getElementById("readingFree");
    if (titleEl) titleEl.value = title || "";
    if (freeEl) freeEl.checked = isFree;

    const editor = initQuill();

    setTimeout(() => {
        if (editor) editor.root.innerHTML = content || "";
    }, 50);

    const modalEl = document.getElementById("editReadingModal");
    if (!modalEl) return;

    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    modal.show();

    // Reset khi modal đóng
    modalEl.addEventListener("hidden.bs.modal", () => {
        if (editor) editor.setContents([]);
        if (titleEl) titleEl.value = "";
        if (freeEl) freeEl.checked = false;
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

        bootstrap.Modal.getInstance(document.getElementById("editReadingModal"))?.hide();
        Toast.show("Lưu bài đọc thành công!", "success");

        // Reload danh sách bài học
        const lessons = await LessonApi.getLessonsByChapter(window.currentLessonEdit.chapterId);
        window.lessonListModule?.renderLessonsIntoChapter(window.currentLessonEdit.chapterId, lessons);

        window.currentLessonEdit = null;
    } catch (err) {
        Toast.show("Lưu thất bại!", "danger");
        console.error(err);
    }
};

// === SỬA BÀI HỌC TỪ NÚT BÚT CHÌ ===
window.editLessonContent = async (chapterId, lessonId) => {
    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);
        if (!lesson || lesson.type !== 1) return;

        window.openReadingModal(
            chapterId,
            lessonId,
            lesson.title,
            lesson.content || "",
            lesson.isFree,
            false // isNew = false
        );
    } catch (err) {
        Toast.show("Không tải được bài học!", "danger");
        console.error(err);
    }
};
