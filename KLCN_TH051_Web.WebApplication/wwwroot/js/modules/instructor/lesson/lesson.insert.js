//// wwwroot/js/modules/instructor/lesson/lesson.insert.js
//import LessonApi from "/js/api/lessonApi.js";
//import Toast from "/js/components/Toast.js";

//let currentChapterId = null;
//let lessonTypesLoaded = false;

//async function loadLessonTypes() {
//    if (lessonTypesLoaded) return;

//    const select = document.getElementById("lessonType");
//    if (!select) return;

//    try {
//        const types = await LessonApi.getLessonTypes();  // DÙNG CHUẨN LessonApi

//        select.innerHTML = '<option value="" disabled selected>Chọn loại bài học</option>';

//        types.forEach(type => {
//            const option = document.createElement("option");
//            option.value = type.value;
//            option.textContent = type.label;
//            select.appendChild(option);
//        });

//        lessonTypesLoaded = true;
//    } catch (err) {
//        console.error("Lỗi load loại bài học:", err);
//        select.innerHTML = '<option value="">Lỗi tải dữ liệu</option>';
//        Toast.show("Không tải được loại bài học!", "danger");
//    }
//}

//document.addEventListener("DOMContentLoaded", () => {
//    setTimeout(initLessonInsert, 100);
//});

//function initLessonInsert() {
//    document.body.addEventListener("click", async (e) => {
//        const btn = e.target.closest(".lesson-insert-btn");
//        if (!btn) return;

//        currentChapterId = btn.dataset.chapterId;

//        // Tự động load loại bài học từ backend qua LessonApi
//        await loadLessonTypes();

//        // Reset form
//        document.getElementById("lessonName").value = "";
//        document.getElementById("lessonType").selectedIndex = 0;
//        document.getElementById("previewCheck").checked = false;
//    });

//    document.getElementById("saveLessonBtn").onclick = createLesson;
//}

//async function createLesson() {
//    const title = document.getElementById("lessonName")?.value.trim();
//    const typeValue = document.getElementById("lessonType")?.value;
//    const isFree = document.getElementById("previewCheck")?.checked ?? false;

//    if (!title || !typeValue) {
//        Toast.show("Vui lòng nhập tên và chọn loại bài học!", "danger", 4000);
//        return;
//    }

//    try {
//        await LessonApi.createLesson(currentChapterId, {
//            title,
//            type: parseInt(typeValue),
//            isFree
//        });

//        bootstrap.Modal.getInstance(document.getElementById("addLessonModal")).hide();

//        const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
//        window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);

//        Toast.show("Tạo bài học thành công!", "success", 3000);
//    } catch (err) {
//        console.error(err);
//        Toast.show("Tạo bài học thất bại!", "danger", 5000);
//    }
//}