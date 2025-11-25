// wwwroot/js/modules/course/course.edit.js
export function editCourse(courseId) {
    if (!courseId) {
        alert("Không có ID khóa học!");
        return;
    }

    // Chuyển hướng sang trang chi tiết khóa học
    window.location.href = `/Instructor/Course/Detail/${courseId}`;
}