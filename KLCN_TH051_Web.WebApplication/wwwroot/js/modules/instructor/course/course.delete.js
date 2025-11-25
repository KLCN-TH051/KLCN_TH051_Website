// wwwroot/js/modules/course/course.delete.js
import CourseApi from "../../../api/courseApi.js";
import Toast from "../../../components/Toast.js";
import { loadTeacherCourses } from "./course.listTeachers.js";

let deleteCourseId = null; // lưu tạm id cần xóa

export function showDeleteModal(id) {
    deleteCourseId = id;
    const deleteModal = new bootstrap.Modal(document.getElementById("deleteModal"));
    deleteModal.show();
}

document.getElementById("btnConfirmDelete").addEventListener("click", async () => {
    if (!deleteCourseId) return;

    try {
        await CourseApi.delete(deleteCourseId);
        Toast.show("Xóa khóa học thành công!", "success");
        // reload danh sách mà không reload toàn trang
        loadTeacherCourses();
    } catch (err) {
        console.error(err);
        Toast.show("Xóa khóa học thất bại!", "danger");
    } finally {
        const deleteModalEl = document.getElementById("deleteModal");
        const modalInstance = bootstrap.Modal.getInstance(deleteModalEl);
        modalInstance.hide();
        deleteCourseId = null;
    }
});

// Cho phép gọi từ HTML
window.showDeleteModal = showDeleteModal;