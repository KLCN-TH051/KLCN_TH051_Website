// Mô-đun xử lý modal Thêm/Sửa phân công giáo viên - môn học
import { loadAssignments } from "./teacherAssignments.list.js";
import { loadTeacherAndSubjectDropdowns } from "./teacherAssignments.dropdown.js";
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";
import Toast from "../../components/Toast.js";

export function initAssignmentModal() {
    const modalEl = document.getElementById("assignmentModal");
    const modal = new bootstrap.Modal(modalEl);

    // Nút Thêm
    document.getElementById("btnAdd").addEventListener("click", () => {
        document.getElementById("modalTitle").innerText = "Thêm phân công";
        document.getElementById("assignmentId").value = "";

        loadTeacherAndSubjectDropdowns().then(() => {
            document.getElementById("teacherSelect").value = "";
            document.getElementById("subjectSelect").value = "";
            modal.show();
        });
    });

    // Nút Sửa (dùng delegation cho table)
    document.querySelector("#tblAssignments tbody").addEventListener("click", (e) => {
        if (e.target.classList.contains("btnEdit")) {
            const button = e.target;
            const id = button.dataset.id;

            if (!id) return;

            // Lấy dữ liệu chi tiết từ server
            TeacherAssignmentsApi.getById(id).then(item => {
                if (!item) {
                    Toast.show("Không tìm thấy phân công!", "danger");
                    return;
                }

                document.getElementById("assignmentId").value = item.id;
                document.getElementById("modalTitle").innerText = "Sửa phân công";

                // Load dropdown trước khi set value
                loadTeacherAndSubjectDropdowns().then(() => {
                    document.getElementById("teacherSelect").value = item.teacherId;
                    document.getElementById("subjectSelect").value = item.subjectId;
                    modal.show();
                });
            });
        }
    });

    // Nút Lưu
    document.getElementById("btnSave").addEventListener("click", () => {
        const id = document.getElementById("assignmentId").value;
        const teacherIdValue = document.getElementById("teacherSelect").value;
        const subjectIdValue = document.getElementById("subjectSelect").value;

        if (!teacherIdValue || !subjectIdValue) {
            Toast.show("Vui lòng chọn giáo viên và môn học!", "danger");
            return; // dừng không gửi request
        }

        const teacherId = parseInt(teacherIdValue);
        const subjectId = parseInt(subjectIdValue);
        const payload = { teacherId, subjectId };

        TeacherAssignmentsApi.getAll().then(allAssignments => {
            const duplicate = allAssignments.some(a =>
                a.teacherId === teacherId &&
                a.subjectId === subjectId &&
                (!id || a.id !== parseInt(id))
            );

            if (duplicate) {
                Toast.show("Giáo viên này đã được phân công môn học này rồi!", "danger");
                return;
            }

            const action = id ? TeacherAssignmentsApi.update(id, payload) : TeacherAssignmentsApi.create(payload);
            action
                .then(res => {
                    if (!res) throw new Error("Lỗi khi lưu phân công"); // nếu API trả null
                    modal.hide();
                    loadAssignments();
                    Toast.show(id ? "Cập nhật phân công thành công!" : "Thêm phân công thành công!", "success");
                })
                .catch(() => Toast.show("Lỗi khi lưu phân công!", "danger"));
        });
    });

    return modal;
}
