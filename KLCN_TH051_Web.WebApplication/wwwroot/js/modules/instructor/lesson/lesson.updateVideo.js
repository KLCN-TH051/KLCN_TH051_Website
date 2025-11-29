// wwwroot/js/modules/instructor/lesson/lesson.updatevideo.js

import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

// ===================== Firebase Config =====================
const firebaseConfig = {
    apiKey: "AIzaSyAPoW_YHRpeV3S5IfU75c2mPZLUVN5h1_o",
    authDomain: "ho-tro-hoc-tap-1bf1f.firebaseapp.com",
    projectId: "ho-tro-hoc-tap-1bf1f",
    storageBucket: "ho-tro-hoc-tap-1bf1f.appspot.com",
    messagingSenderId: "776893455274",
    appId: "1:776893455274:web:84536ab780a29d14e7e9d3",
    measurementId: "G-70838HXZZJ"
};

const app = initializeApp(firebaseConfig);
const storage = getStorage(app);

let currentLessonEdit = null;

// ===================== UPLOAD VIDEO =====================
async function uploadVideo(file) {
    if (!file) throw "Chưa chọn file video";

    return new Promise((resolve, reject) => {
        const storageRef = ref(storage, `lessons/${Date.now()}_${file.name}`);
        const uploadTask = uploadBytesResumable(storageRef, file);

        uploadTask.on('state_changed',
            snapshot => {
                const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
                console.log(`Upload is ${progress.toFixed(2)}% done`);
            },
            error => reject(error),
            async () => {
                try {
                    const url = await getDownloadURL(uploadTask.snapshot.ref);
                    resolve(url);
                } catch (err) {
                    reject(err);
                }
            }
        );
    });
}

// ===================== MỞ MODAL VIDEO =====================
window.openVideoModal = async (chapterId, lessonId) => {
    currentLessonEdit = { chapterId, lessonId };

    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

        document.getElementById("videoTitle").value = lesson.title || "";
        document.getElementById("videoPreview").src = lesson.videoUrl || "";
        const freeEl = document.getElementById("editVideoFree");
        if (freeEl) freeEl.checked = lesson.isFree || false;
        document.getElementById("videoUploadInput").value = "";

        const modalEl = document.getElementById("editVideoModal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        // Reset modal khi đóng
        modalEl.addEventListener("hidden.bs.modal", () => {
            document.getElementById("videoTitle").value = "";
            document.getElementById("videoPreview").src = "";
            if (freeEl) freeEl.checked = false;
            document.getElementById("videoUploadInput").value = "";
            currentLessonEdit = null;
        }, { once: true });

    } catch (err) {
        console.error(err);
        Toast.show("Không tải được thông tin video!", "danger");
    }
};

// ===================== LƯU VIDEO =====================
window.saveVideo = async () => {
    if (!currentLessonEdit) return;

    const title = document.getElementById("videoTitle").value.trim();
    const fileInput = document.getElementById("videoUploadInput");
    const file = fileInput.files[0];
    const isFree = document.getElementById("editVideoFree").checked;

    if (!title) {
        Toast.show("Chưa nhập tiêu đề video!", "warning");
        return;
    }

    try {
        let videoUrl = document.getElementById("videoPreview").src;
        if (file) videoUrl = await uploadVideo(file);

        await LessonApi.updateLessonVideo(currentLessonEdit.chapterId, currentLessonEdit.lessonId, {
            title,
            videoUrl,
            isFree
        });

        // Reload danh sách bài học
        const lessons = await LessonApi.getLessonsByChapter(currentLessonEdit.chapterId);
        window.lessonListModule?.renderLessonsIntoChapter(currentLessonEdit.chapterId, lessons);

        Toast.show("Cập nhật video thành công!", "success");
        bootstrap.Modal.getInstance(document.getElementById("editVideoModal"))?.hide();

    } catch (err) {
        console.error(err);
        Toast.show("Cập nhật video thất bại!", "danger");
    }
};
