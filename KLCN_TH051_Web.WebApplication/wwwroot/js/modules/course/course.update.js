import BaseApi from "../../core/BaseApi.js";
import CourseApi from "../../api/courseApi.js";
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";
import Toast from "../../components/Toast.js";



const courseId = window.location.pathname.split("/").pop();

const courseImageInput = document.getElementById("courseImage");
const previewImg = document.createElement("img");
previewImg.style.maxWidth = "200px";
previewImg.style.display = "block";
previewImg.style.marginTop = "10px";
courseImageInput.parentNode.appendChild(previewImg);

courseImageInput.addEventListener("change", () => {
    const file = courseImageInput.files[0];
    if (file) {
        previewImg.src = URL.createObjectURL(file);
    }
});

let currentImage = null; // lưu tên ảnh hiện tại

async function loadCourseDetail() {
    if (!courseId) return;

    try {
        const course = await CourseApi.getById(courseId);
        if (!course) {
            Toast.show("Không tìm thấy khóa học!", "danger");
            return;
        }

        document.getElementById("courseName").value = course.name;
        document.getElementById("coursePrice").value = course.price ?? "";
        document.getElementById("courseDescription").value = course.description ?? "";
        document.getElementById("freeCheck").checked = course.isFree ?? false;

        currentImage = course.image ?? null;
        previewImg.src = currentImage ? `/images/courses/${currentImage}` : "";

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

    } catch (err) {
        console.error(err);
        Toast.show("Lỗi khi tải thông tin khóa học!", "danger");
    }
}

async function uploadCourseImage() {
    if (courseImageInput.files.length === 0) return currentImage; // giữ ảnh cũ nếu không đổi
    const file = courseImageInput.files[0];
    const allowedTypes = ["image/jpeg", "image/png", "image/gif"];
    if (!allowedTypes.includes(file.type)) {
        Toast.show("Chỉ cho phép file JPG, PNG, GIF", "warning");
        return null;
    }

    const formData = new FormData();
    formData.append("file", file);

    try {
        // Dùng BaseApi, truyền option isFormData: true
        const result = await BaseApi.post("upload/courseimage", formData, { isFormData: true });
        return result.fileName;
    } catch (err) {
        console.error(err);
        Toast.show("Upload ảnh thất bại!", "danger");
        return null;
    }
}

document.getElementById("courseForm").addEventListener("submit", async e => {
    e.preventDefault();

    const name = document.getElementById("courseName").value.trim();
    const subjectId = document.getElementById("courseCategory").value;
    const price = Number(document.getElementById("coursePrice").value);
    const description = document.getElementById("courseDescription").value;
    const isFree = document.getElementById("freeCheck").checked;

    if (!name || !subjectId) {
        Toast.show("Vui lòng nhập đầy đủ tên khóa học và chọn môn học!", "warning");
        return;
    }

    // Upload ảnh nếu có, hoặc giữ ảnh cũ
    const imageName = await uploadCourseImage() || currentImage;

    const data = {
        name,
        subjectId,
        price,
        description,
        isFree,
        thumbnail: imageName  // dùng imageName mới thay vì currentImage
    };

    try {
        await CourseApi.update(courseId, data);
        currentImage = imageName; // cập nhật currentImage mới
        Toast.show("Cập nhật khóa học thành công!", "success");
        previewImg.src = currentImage ? `/images/courses/${currentImage}` : "";
    } catch (err) {
        console.error(err);
        Toast.show("Đã có lỗi xảy ra khi cập nhật khóa học!", "danger");
    }
});



document.getElementById("submitCourseButton").addEventListener("click", async () => {
    try {
        await CourseApi.updateStatus(courseId, 1);
        Toast.show("Khóa học đã được gửi duyệt!", "success");
    } catch (err) {
        console.error(err);
        Toast.show("Đã có lỗi xảy ra khi gửi khóa học!", "danger");
    }
});

document.addEventListener("DOMContentLoaded", loadCourseDetail);
