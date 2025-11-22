import CourseApi from "../../api/courseApi.js";
import { getStatusBadge, formatPrice } from "./course.utils.js";
import { editCourse } from "./course.edit.js";
window.editCourse = editCourse;

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
                   <button class="btn btn-sm btn-danger" data-id="${course.id}" onclick="showDeleteModal(${course.id})">Xóa</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error("Lỗi loadTeacherCourses:", err);
        tbody.innerHTML = `<tr><td colspan="4" class="text-danger text-center">Lỗi khi tải dữ liệu</td></tr>`;
    }
}

document.addEventListener("DOMContentLoaded", loadTeacherCourses);
