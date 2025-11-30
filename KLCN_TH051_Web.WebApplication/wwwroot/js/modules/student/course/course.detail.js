//import CourseApi from "../../../api/courseApi.js";
//import UploadApi from "../../../api/uploadApi.js";

//document.addEventListener("DOMContentLoaded", async () => {
//    // Lấy courseId từ query string
//    const params = new URLSearchParams(window.location.search);
//    const courseId = params.get("id");

//    if (!courseId) return console.error("Không có courseId trong URL");

//    try {
//        const response = await CourseApi.getById(courseId);
//        const course = response.data; // { id, name, description, thumbnail, price, teacherName, subjectName }

//        // Header
//        document.querySelector(".course-title").textContent = course.name;
//        document.querySelector(".course-description").textContent = course.description;
//        document.querySelector(".instructor-name").textContent = course.teacherName;
//        document.querySelector(".course-category-tag").textContent = course.subjectName;

//        // Ảnh khóa học
//        const courseImgEl = document.querySelector(".course-image");
//        if (course.thumbnail) {
//            courseImgEl.src = UploadApi.getFileUrl("banner", course.thumbnail);
//        }

//        // Giá
//        const priceEl = document.querySelector(".course-price");
//        priceEl.textContent = course.price > 0
//            ? new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(course.price)
//            : "Miễn phí";

//    } catch (err) {
//        console.error("Lỗi khi load course detail:", err);
//        alert("Không thể load chi tiết khóa học");
//    }
//});
