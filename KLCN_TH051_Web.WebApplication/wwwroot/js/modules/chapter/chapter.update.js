import ChapterApi from "../../api/chapterApi.js";
import Toast from "../../components/Toast.js";
import { loadChapterList } from "./chapter.loadList.js";

let editingChapterId = null;

// ==============================
// Mở popup và load dữ liệu
// ==============================
export async function editChapter(id) {
    const courseId = window.location.pathname.split("/").pop();

    try {
        const chapter = await ChapterApi.getById(courseId, id);
        if (!chapter) throw new Error("Không tìm thấy chương");

        editingChapterId = id;

        document.getElementById("editChapterName").value = chapter.name;

        const modalEl = document.getElementById("updateChapterModal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    } catch (err) {
        console.error(err);
        Toast.show("Không thể tải dữ liệu chương!", "danger");
    }
}

// ==============================
// Lưu chương đã sửa
// ==============================
export async function updateChapter() {
    if (!editingChapterId) return;

    const courseId = window.location.pathname.split("/").pop();
    const nameInput = document.getElementById("editChapterName");
    const name = nameInput.value.trim();

    if (!name) {
        Toast.show("Tên chương không được để trống!", "warning");
        nameInput.classList.add("is-invalid");
        nameInput.focus();
        return;
    }

    nameInput.classList.remove("is-invalid");

    try {
        await ChapterApi.update(courseId, editingChapterId, { name });

        Toast.show("Cập nhật chương thành công!", "success");

        const modalEl = document.getElementById("updateChapterModal");
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();

        await loadChapterList();
    } catch (err) {
        console.error(err);
        Toast.show("Cập nhật chương thất bại!", "danger");
    }
}

window.editChapter = editChapter;
window.updateChapter = updateChapter;
