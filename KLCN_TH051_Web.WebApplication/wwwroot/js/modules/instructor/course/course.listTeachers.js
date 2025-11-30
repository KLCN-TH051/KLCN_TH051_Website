import CourseApi from "../../../api/courseApi.js";
import UploadApi from "../../../api/uploadApi.js"; // ⚠ cần import thêm
import { getStatusBadge, formatPrice } from "./course.utils.js";
import { editCourse } from "./course.edit.js";
window.editCourse = editCourse;

export async function loadTeacherCourses() {
    const tbody = document.querySelector("table tbody");
    tbody.innerHTML = `<tr><td colspan="5" class="text-center">Đang tải...</td></tr>`;

    try {
        const courses = await CourseApi.getTeacherCourses();
        if (!courses || courses.length === 0) {
            tbody.innerHTML = `<tr><td colspan="5" class="text-center">Chưa có khóa học nào</td></tr>`;
            return;
        }

        tbody.innerHTML = "";
        courses.forEach(course => {

            // ==== 🔥 LẤY URL THUMBNAIL ====
            const thumbnailUrl =
                course.thumbnailUrl ||
                UploadApi.getFileUrl("course", course.thumbnail) ||
                "/images/default-course.png";

            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td style="width:120px">
                    <img src="${thumbnailUrl}"
                         alt="${course.name}"
                         class="img-thumbnail"
                         style="width:100px; height:60px; object-fit:cover;">
                </td>
                <td>${course.name}</td>
                <td>${formatPrice(course.price)}</td>
                <td>${getStatusBadge(course.status)}</td>
                <td class="text-center">
                    <button class="btn btn-sm btn-info me-1" onclick="editCourse(${course.id})">Sửa</button>
                    <button class="btn btn-sm btn-danger" onclick="showDeleteModal(${course.id})">Xóa</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error("Lỗi loadTeacherCourses:", err);
        tbody.innerHTML = `<tr><td colspan="5" class="text-danger text-center">Lỗi khi tải dữ liệu</td></tr>`;
    }
}

document.addEventListener("DOMContentLoaded", loadTeacherCourses);
