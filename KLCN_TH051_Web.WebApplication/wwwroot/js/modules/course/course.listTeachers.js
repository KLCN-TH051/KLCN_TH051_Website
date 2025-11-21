import CourseApi from "../../api/courseApi.js";
import { editCourse } from "./course.edit.js"; 
window.editCourse = editCourse;


// ------------------------------
// Format trạng thái
// ------------------------------
function getStatusBadge(status) {
    const map = {
        0: "Chờ duyệt",
        1: "Đã duyệt",
        2: "Từ chối",
        3: "Bản nháp"
    };

    const color = {
        0: "warning",
        1: "success",
        2: "danger",
        3: "secondary"
    };

    return `<span class="badge bg-${color[status] || "secondary"}">${map[status] || "Không rõ"}</span>`;
}

// ------------------------------
// Format giá
// ------------------------------
function formatPrice(num) {
    return num ? num.toLocaleString("vi-VN") + "đ" : "0đ";
}

// ------------------------------
// Load danh sách khóa học của giáo viên
// ------------------------------
export async function loadTeacherCourses() {
    const tbody = document.querySelector("table tbody");
    tbody.innerHTML = `<tr><td colspan="4" class="text-center">Đang tải...</td></tr>`;

    try {
        const courses = await CourseApi.getTeacherCourses();
        if (!courses || courses.length === 0) {
            tbody.innerHTML = `<tr><td colspan="4" class="text-center">Chưa có khóa học nào</td></tr>`;
            return;
        }

        tbody.innerHTML = "";
        courses.forEach(course => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${course.name}</td>
                <td>${formatPrice(course.price)}</td>
                <td>${getStatusBadge(course.status)}</td>
                <td class="text-center">
                    <button class="btn btn-sm btn-info me-1" data-id="${course.id}" onclick="editCourse(${course.id})">Sửa</button>
                    <button class="btn btn-sm btn-danger" data-id="${course.id}" onclick="deleteCourse(${course.id})">Xóa</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error("Lỗi loadTeacherCourses:", err);
        tbody.innerHTML = `<tr><td colspan="4" class="text-danger text-center">Lỗi khi tải dữ liệu</td></tr>`;
    }
}

// ------------------------------
// Load khi trang sẵn sàng
// ------------------------------
document.addEventListener("DOMContentLoaded", () => {
    loadTeacherCourses();
});
