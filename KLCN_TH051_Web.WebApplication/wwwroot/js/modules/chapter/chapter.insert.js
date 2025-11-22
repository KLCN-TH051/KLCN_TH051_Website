// ========================== chapter.insert.js ==========================
import ChapterApi from "../../api/chapterApi.js";
import Toast from "../../components/Toast.js";
import { loadChapterList } from "./chapter.loadList.js";

// Lấy courseId từ URL
const courseId = window.location.pathname.split("/").pop();

// ==============================
// Hàm thêm chương
// ==============================
export async function addChapter() {
    const nameInput = document.getElementById("addChapterName");

    const name = nameInput.value.trim();

    // Validate
    if (!name) {
        Toast.show("Vui lòng nhập tên chương!", "warning");
        nameInput.classList.add("is-invalid");
        nameInput.focus();
        return;
    }
    nameInput.classList.remove("is-invalid");

    try {
        // Gửi lên API
        await ChapterApi.create(courseId, { name });

        Toast.show("Tạo chương mới thành công!", "success");

        // Reset input
        nameInput.value = "";

        // Đóng modal
        const modalEl = document.getElementById("addChapterModal");
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();

        // Load lại list
        await loadChapterList();
    } catch (err) {
        console.error(err);
        Toast.show("Tạo chương thất bại!", "danger");
    }
}

// Gán global để gọi từ HTML onclick
window.addChapter = addChapter;
