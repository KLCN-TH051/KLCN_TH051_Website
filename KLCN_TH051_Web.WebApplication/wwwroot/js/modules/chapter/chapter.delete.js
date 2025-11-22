import ChapterApi from "../../api/chapterApi.js";
import Toast from "../../components/Toast.js";
import { loadChapterList } from "./chapter.loadList.js";

// Lấy courseId từ URL
const courseId = window.location.pathname.split("/").pop();

// Biến lưu id chapter đang muốn xóa
let chapterToDelete = null;

// ==============================
// Mở popup xóa chapter
// ==============================
export function deleteChapter(id) {
    chapterToDelete = id;

    const modalEl = document.getElementById("deleteChapterModal");
    const modal = new bootstrap.Modal(modalEl);
    modal.show();
}

// ==============================
// Xác nhận xóa trong modal
// ==============================
const btnConfirmDelete = document.getElementById("btnConfirmDeleteChapter");
btnConfirmDelete.addEventListener("click", async () => {
    if (!chapterToDelete) return;

    try {
        await ChapterApi.delete(courseId, chapterToDelete);
        Toast.show("Xóa chương thành công!", "success");

        // Đóng modal
        const modalEl = document.getElementById("deleteChapterModal");
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();

        // Reset biến xóa
        chapterToDelete = null;

        // Load lại danh sách chương
        await loadChapterList();
    } catch (err) {
        console.error("Lỗi khi xóa chương:", err);
        Toast.show("Xóa chương thất bại!", "danger");
    }
});

// Gán ra global để gọi từ onclick button trong HTML
window.deleteChapter = deleteChapter;
