// wwwroot/js/modules/course/course.insert.js
import CourseApi from "../../api/courseApi.js";
import TeacherAssignmentsApi from "../../api/TeacherAssignments.js";
import Toast from "../../components/Toast.js"; // import Toast.js

// ------------------------------
// Load danh sách môn học vào combobox
// ------------------------------
async function loadSubjects() {
    const categorySelect = document.getElementById("categorySelect");
    categorySelect.innerHTML = `<option value="">Đang tải...</option>`;

    try {
        const teacherId = localStorage.getItem("teacherId");
        if (!teacherId) throw new Error("Chưa có teacherId trong localStorage");

        const subjects = await TeacherAssignmentsApi.getSubjectsByTeacher(teacherId);

        categorySelect.innerHTML = `<option value="">Chọn môn học</option>`;

        if (subjects && subjects.length > 0) {
            subjects.forEach(sub => {
                const opt = document.createElement("option");
                opt.value = sub.subjectId;
                opt.textContent = sub.subjectName;
                categorySelect.appendChild(opt);
            });
        } else {
            categorySelect.innerHTML = `<option value="">Không có môn học</option>`;
        }
    } catch (err) {
        console.error("Lỗi loadSubjects:", err);
        categorySelect.innerHTML = `<option value="">Không tải được môn học</option>`;
    }
}

// ------------------------------
// Mở modal thêm khóa học
// ------------------------------
const courseModalEl = document.getElementById("courseModal");
const courseModal = new bootstrap.Modal(courseModalEl);
document.querySelector('[data-bs-target="#courseModal"]').addEventListener("click", () => {
    document.getElementById("courseForm").reset();
    document.getElementById("courseId").value = "";
    document.getElementById("courseModalLabel").textContent = "Thêm khóa học";
    loadSubjects();
    courseModal.show();
});

// ------------------------------
// Lưu khóa học
// ------------------------------
document.getElementById("btnSubmitCourse").addEventListener("click", async (e) => {
    e.preventDefault();

    const courseId = document.getElementById("courseId").value;
    const name = document.getElementById("courseName").value.trim();
    const subjectId = document.getElementById("categorySelect").value;

    if (!name || !subjectId) {
        Toast.show("Vui lòng nhập đầy đủ tên khóa học và chọn môn học!", "warning");
        return;
    }

    const data = { name, subjectId };

    try {
        let result;
        if (courseId) {
            // Cập nhật khóa học
            result = await CourseApi.update(courseId, data);
            Toast.show("Cập nhật khóa học thành công!", "success");
        } else {
            // Tạo mới khóa học (draft)
            result = await CourseApi.createDraft(data);
            Toast.show("Tạo khóa học thành công!", "success");
        }

        courseModal.hide();

        // Lấy ID khóa học mới tạo hoặc cập nhật
        const newCourseId = courseId || result?.id;

        if (newCourseId) {
            // Chuyển hướng sang trang detail
            window.location.href = `/Instructor/Course/Detail/${newCourseId}`;
        } else {
            // Nếu không có ID, reload danh sách
            const event = new Event("reloadTeacherCourses");
            document.dispatchEvent(event);
        }

    } catch (err) {
        console.error("Lỗi khi lưu khóa học:", err);
        Toast.show("Đã có lỗi xảy ra khi lưu khóa học!", "danger");
    }
});

// ------------------------------
// Export loadSubjects
// ------------------------------
export { loadSubjects };
