// wwwroot/js/modules/instructor/lesson/lesson.updateVideo.js
import LessonApi from "/js/api/lessonApi.js";
import UploadApi from "/js/api/uploadApi.js";
import Toast from "/js/components/Toast.js";

let currentChapterId = null;
let currentLessonId = null;

// ==================== MỞ MODAL VIDEO ====================
window.openVideoModal = function (chapterId, lessonId, title, videoUrl, isFree, isNew) {
    currentChapterId = chapterId;
    currentLessonId = lessonId;

    // Điền dữ liệu vào modal
    document.querySelector("#videoLessonTitle").value = title || "";
    document.querySelector("#videoLessonFree").checked = !!isFree;

    const videoPreview = document.querySelector("#videoPreview");
    videoPreview.innerHTML = "";

    if (videoUrl && videoUrl.length > 0) {
        videoPreview.innerHTML = `
            <video controls style="max-width: 100%">
                <source src="${videoUrl}" type="video/mp4">
            </video>
        `;
    }

    document.querySelector("#videoModalTitle").innerText =
        isNew ? "Tạo bài học video" : "Chỉnh sửa bài học video";

    bootstrap.Modal.getOrCreateInstance(
        document.getElementById("editVideoModal")
    ).show();
};

// ==================== LƯU VIDEO + UPLOAD ====================
window.saveVideoLesson = async function () {
    const title = document.querySelector("#videoLessonTitle").value.trim();
    const isFree = document.querySelector("#videoLessonFree").checked;
    const inputFile = document.querySelector("#videoFile");
    const file = inputFile.files[0];

    if (!title) {
        Toast.show("Vui lòng nhập tiêu đề bài học!", "danger");
        return;
    }

    let uploadedVideoFileName = null;

    if (file) {
        // Upload video nếu có file mới
        try {
            const upload = await UploadApi.uploadFile(file, "video");
            uploadedVideoFileName = upload.fileName;

            const url = UploadApi.getFileUrl("video", uploadedVideoFileName);
            document.querySelector("#videoPreview").innerHTML = `
                <video controls style="max-width:100%">
                    <source src="${url}" type="video/mp4">
                </video>
            `;
            Toast.show("Upload video thành công!", "success");

        } catch (err) {
            console.error(err);
            Toast.show("Upload video thất bại!", "danger");
            return; // dừng lưu bài học nếu upload thất bại
        }
    } else {
        // Nếu không chọn file mới, giữ nguyên videoUrl cũ từ preview
        const previewVideo = document.querySelector("#videoPreview video source");
        if (previewVideo) {
            uploadedVideoFileName = previewVideo.src.split("/").pop();
        }
    }

    if (!uploadedVideoFileName) {
        Toast.show("Bạn chưa chọn video!", "danger");
        return;
    }

    // Cập nhật bài học
    try {
        await LessonApi.updateLessonVideo(currentChapterId, currentLessonId, {
            title: title,
            isFree: isFree,
            videoUrl: uploadedVideoFileName,
            durationMinutes: null
        });

        Toast.show("Lưu bài học video thành công!", "success");

        // Cập nhật lại danh sách bài học
        const lessons = await LessonApi.getLessonsByChapter(currentChapterId);
        window.lessonListModule.renderLessonsIntoChapter(currentChapterId, lessons);

        bootstrap.Modal.getInstance(
            document.getElementById("editVideoModal")
        ).hide();

    } catch (err) {
        console.error(err);
        Toast.show("Lưu bài học video thất bại!", "danger");
    }
};
