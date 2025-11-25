// wwwroot/js/modules/teacherAssignments/teacherAssignments.delete.js
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";
import { loadAssignments } from "./teacherAssignments.list.js";
import Toast from "../../components/Toast.js";

export function initDeleteModal() {
    const modalEl = document.getElementById("deleteModal");
    const modal = new bootstrap.Modal(modalEl);
    let deleteId = null;

    document.querySelector("#tblAssignments tbody").addEventListener("click", e => {
        if (e.target.classList.contains("btnDelete")) {
            deleteId = e.target.dataset.id;
            modal.show();
        }
    });

    document.getElementById("btnDeleteConfirm").addEventListener("click", () => {
        if (!deleteId) return;
        TeacherAssignmentsApi.remove(deleteId)
            .then(() => {
                modal.hide();
                deleteId = null;
                loadAssignments();
                Toast.show("Xóa phân công thành công!", "success");
            })
            .catch(err => Toast.show("Lỗi khi xóa phân công!", "danger"));
    });

    return modal;
}
