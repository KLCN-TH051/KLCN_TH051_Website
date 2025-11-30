import CourseApi from "../../../api/courseApi.js";
import UploadApi from "../../../api/uploadApi.js";

document.addEventListener("DOMContentLoaded", async () => {
    // Lấy courseId từ URL
    const pathParts = window.location.pathname.split("/");
    const courseId = pathParts[pathParts.length - 1]; // ID cuối cùng

    if (!courseId) return console.error("Không tìm thấy courseId trong URL");

    try {
        const course = await CourseApi.getById(courseId);

        // Header
        document.querySelector(".course-title").textContent = course.name;
        document.querySelector(".course-description").textContent = course.description;
        document.querySelector(".instructor-name").textContent = course.teacherName ?? "Chưa cập nhật";
        document.querySelector(".course-category-tag").textContent = course.subjectName ?? "Chưa cập nhật";

        // Ảnh khóa học
        const courseImgEl = document.querySelector(".course-image");
        if (course.thumbnail) {
            courseImgEl.src = UploadApi.getFileUrl("banner", course.thumbnail)
                || "https://placehold.co/500x250?text=No+Image";
        } else {
            courseImgEl.src = "https://placehold.co/500x250?text=No+Image";
        }

        // Giá
        const priceEl = document.querySelector(".course-price");
        priceEl.textContent = course.price > 0
            ? new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(course.price)
            : "Miễn phí";

    } catch (err) {
        console.error("Lỗi khi load chi tiết khóa học:", err);
        alert("Không thể load chi tiết khóa học");
    }
});
