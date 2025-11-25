// wwwroot/js/modules/teacherAssignments/teacherAssignments.list.js
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";

export function loadAssignments() {
    return TeacherAssignmentsApi.getAll()
        .then(data => {
            const tbody = document.querySelector("#tblAssignments tbody");
            tbody.innerHTML = "";

            if (!data || data.length === 0) {
                tbody.innerHTML = `<tr>
                    <td colspan="4" class="text-center">Không có dữ liệu</td>
                </tr>`;
                return;
            }

            data.forEach(item => {
                tbody.innerHTML += `<tr>
                    <td>${item.id}</td>
                    <td>${item.teacherName}</td>
                    <td>${item.subjectName}</td>
                    <td class="text-center">
                        <button class="btn btn-warning btn-sm btnEdit" data-id="${item.id}">Sửa</button>
                        <button class="btn btn-danger btn-sm btnDelete" data-id="${item.id}">Xóa</button>
                    </td>
                </tr>`;
            });
        })
        .catch(err => console.error("Lỗi load assignments:", err));
}
