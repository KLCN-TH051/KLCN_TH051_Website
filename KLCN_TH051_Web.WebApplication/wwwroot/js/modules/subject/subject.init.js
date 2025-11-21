// modules/subject/subject.init.js
import { loadSubjects } from "./subject.list.js";
import { insertSubject } from "./subject.insert.js";
import { updateSubject } from "./subject.update.js";
import { deleteSubject } from "./subject.delete.js";

export function initSubjectModule() {
    let editId = null;
    let deleteId = null;

    const subjectModalEl = document.getElementById("subjectModal");
    const deleteModalEl = document.getElementById("deleteModal");

    const subjectModal = new bootstrap.Modal(subjectModalEl);
    const deleteModal = new bootstrap.Modal(deleteModalEl);

    // Load danh sách môn học
    loadSubjects();

    // Thêm môn học
    document.getElementById("btnAdd").addEventListener("click", () => {
        editId = null;
        document.getElementById("modalTitle").innerText = "Thêm môn học";
        document.getElementById("subjectNameInput").value = "";
        document.getElementById("subjectDescInput").value = "";
        subjectModal.show();
    });

    // Lưu thêm/sửa
    document.getElementById("btnSave").addEventListener("click", () => {
        const nameInput = document.getElementById("subjectNameInput").value.trim();
        if (!nameInput) return;

        const action = editId ? updateSubject(editId) : insertSubject();
        action.then(() => subjectModal.hide());
    });

    // Bắt sự kiện nút Sửa trong bảng
    document.getElementById("subjectTableBody").addEventListener("click", e => {
        if (e.target.dataset.edit) {
            const sub = JSON.parse(e.target.dataset.edit);
            editId = sub.id;

            document.getElementById("modalTitle").innerText = "Sửa môn học";
            document.getElementById("subjectNameInput").value = sub.name;
            document.getElementById("subjectDescInput").value = sub.description || "";

            subjectModal.show();
        }
    });

    // Bắt sự kiện nút Xóa trong bảng
    document.getElementById("subjectTableBody").addEventListener("click", e => {
        if (e.target.dataset.delete) {
            deleteId = e.target.dataset.delete;
            deleteModal.show();
        }
    });

    // Xác nhận xóa
    document.getElementById("btnConfirmDelete").addEventListener("click", () => {
        if (!deleteId) return;

        deleteSubject(deleteId)
            .then(() => {
                deleteModal.hide();
                deleteId = null;
            })
            .catch(() => {
                // modal vẫn mở nếu xóa lỗi
            });
    });
}
