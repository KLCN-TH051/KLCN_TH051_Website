import { loadAssignments } from "./teacherAssignments.list.js";
import { initAssignmentModal } from "./teacherAssignments.modal.js";
import { initDeleteModal } from "./teacherAssignments.delete.js";

export function initTeacherAssignmentsModule() {
    loadAssignments();       // Load table
    initAssignmentModal();   // Modal đã tự lắng nghe Thêm/Sửa
    initDeleteModal();       // Xóa
}
