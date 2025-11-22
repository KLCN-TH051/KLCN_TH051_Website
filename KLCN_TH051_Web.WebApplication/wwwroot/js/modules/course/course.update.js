import BaseApi from "../../core/BaseApi.js";
import CourseApi from "../../api/courseApi.js";
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";
import Toast from "../../components/Toast.js";
import { getStatusBadge } from "../../modules/course/course.utils.js";

const courseId = window.location.pathname.split("/").pop();

// ==============================
// Element input và preview
// ==============================
const courseImageInput = document.getElementById("courseImage");
const previewImg = document.createElement("img");
previewImg.style.maxWidth = "200px";
previewImg.style.display = "block";
previewImg.style.marginTop = "10px";
courseImageInput.parentNode.appendChild(previewImg);

let currentImage = null; // lưu tên ảnh hiện tại

// ==============================
// Hiển thị preview khi chọn file mới
// ==============================
courseImageInput.addEventListener("change", () => {
    const file = courseImageInput.files[0];
    if (file) {
        previewImg.src = URL.createObjectURL(file);
    } else {
        previewImg.src = currentImage ? BaseApi.getFileUrl(`images/courses/${currentImage}`) : "";
    }
});

// ==============================
// Load chi tiết khóa học
// ==============================
async function loadCourseDetail() {
    if (!courseId) return;

    try {
        const course = await CourseApi.getById(courseId);
        if (!course) {
            Toast.show("Không tìm thấy khóa học!", "danger");
            return;
        }

        // Điền dữ liệu vào form
        document.getElementById("courseName").value = course.name;
        document.getElementById("coursePrice").value = course.price ?? "";
        document.getElementById("courseDescription").value = course.description ?? "";

        currentImage = course.thumbnail ?? null;
        previewImg.src = currentImage ? BaseApi.getFileUrl(`images/courses/${currentImage}`) : "";

        // Set giá trị startDate & endDate
        document.getElementById("startDate").value = course.startDate
            ? new Date(course.startDate).toISOString().slice(0, 16)
            : "";
        document.getElementById("endDate").value = course.endDate
            ? new Date(course.endDate).toISOString().slice(0, 16)
            : "";

        // Load danh sách môn học của giáo viên
        const teacherId = localStorage.getItem("teacherId");
        const subjects = await TeacherAssignmentsApi.getSubjectsByTeacher(teacherId);
        const select = document.getElementById("courseCategory");
        select.innerHTML = `<option value="">-- Chọn môn học --</option>`;
        subjects.forEach(sub => {
            const opt = document.createElement("option");
            opt.value = sub.subjectId;
            opt.textContent = sub.subjectName;
            if (sub.subjectId === course.subjectId) opt.selected = true;
            select.appendChild(opt);
        });

        // Hiển thị trạng thái khóa học
        const statusSpan = document.getElementById("courseStatus");
        if (statusSpan) statusSpan.innerHTML = getStatusBadge(course.status);

        // Hiển thị / ẩn nút gửi khóa học
        const submitBtn = document.getElementById("submitCourseButton");
        if (submitBtn) {
            if (course.status === 3) { // Bản nháp
                submitBtn.style.display = "inline-block";
            } else {
                submitBtn.style.display = "none";
            }
        }

    } catch (err) {
        console.error(err);
        Toast.show("Lỗi khi tải thông tin khóa học!", "danger");
    }
}

// ==============================
// Upload file image lên server
// ==============================
async function uploadCourseImage() {
    if (courseImageInput.files.length === 0) return null;
    const file = courseImageInput.files[0];

    const allowedTypes = ["image/jpeg", "image/png", "image/gif"];
    if (!allowedTypes.includes(file.type)) {
        Toast.show("Chỉ cho phép file JPG, PNG, GIF", "warning");
        return null;
    }

    const formData = new FormData();
    formData.append("file", file);

    try {
        const result = await BaseApi.post("upload/courseimage", formData, { isFormData: true });
        return result.fileName;
    } catch (err) {
        console.error(err);
        Toast.show("Upload ảnh thất bại!", "danger");
        return null;
    }
}

// ==============================
// Submit form cập nhật khóa học
// ==============================
document.getElementById("courseForm").addEventListener("submit", async e => {
    e.preventDefault();

    const name = document.getElementById("courseName").value.trim();
    const subjectId = document.getElementById("courseCategory").value;
    const price = Number(document.getElementById("coursePrice").value);
    const description = document.getElementById("courseDescription").value;
    const startDate = document.getElementById("startDate").value;
    const endDate = document.getElementById("endDate").value;

    if (!name || !subjectId) {
        Toast.show("Vui lòng nhập đầy đủ tên khóa học và chọn môn học!", "warning");
        return;
    }

    const newImage = await uploadCourseImage();
    const imageName = newImage || currentImage;

    const data = {
        name,
        subjectId,
        price,
        description,
        thumbnail: imageName,
        startDate: startDate ? new Date(startDate).toISOString() : null,
        endDate: endDate ? new Date(endDate).toISOString() : null
    };

    try {
        await CourseApi.update(courseId, data);
        Toast.show("Cập nhật khóa học thành công!", "success");
        await loadCourseDetail(); // tự động load lại
    } catch (err) {
        console.error(err);
        Toast.show("Đã có lỗi xảy ra khi cập nhật khóa học!", "danger");
    }
});

// ==============================
// Gửi duyệt khóa học
// ==============================
document.getElementById("submitCourseButton").addEventListener("click", async () => {
    try {
        await CourseApi.submit(courseId);
        Toast.show("Khóa học đã được gửi duyệt!", "success");
        await loadCourseDetail(); // tự động load lại
    } catch (err) {
        console.error("Lỗi khi submit khóa học:", err);
        Toast.show("Đã có lỗi xảy ra khi gửi khóa học!", "danger");
    }
});

// ==============================
// Load chi tiết khi DOM sẵn sàng
// ==============================
document.addEventListener("DOMContentLoaded", loadCourseDetail);
