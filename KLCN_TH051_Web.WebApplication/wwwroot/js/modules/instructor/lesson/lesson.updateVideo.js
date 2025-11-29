// wwwroot/js/modules/instructor/lesson/lesson.updatevideo.js
import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

// ==========================
// Firebase config chuẩn
// ==========================
const firebaseConfig = {
    apiKey: "AIzaSyAPoW_YHRpeV3S5IfU75c2mPZLUVN5h1_o",
    authDomain: "ho-tro-hoc-tap-1bf1f.firebaseapp.com",
    projectId: "ho-tro-hoc-tap-1bf1f",
    storageBucket: "ho-tro-hoc-tap-1bf1f.firebasestorage.app",
    messagingSenderId: "776893455274",
    appId: "1:776893455274:web:84536ab780a29d14e7e9d3"
};

const app = initializeApp(firebaseConfig);
const storage = getStorage(app);

let currentLessonEdit = null;

// ==========================
// Upload file video với progress
// ==========================
async function uploadVideo(file, onProgress) {
    if (!file) throw new Error("Không có file");

    return new Promise((resolve, reject) => {
        const filePath = `lessons/${Date.now()}_${file.name}`;
        const storageRef = ref(storage, filePath);
        const uploadTask = uploadBytesResumable(storageRef, file);

        uploadTask.on('state_changed',
            snapshot => {
                const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
                onProgress?.(progress);
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

// ==========================
// Mở modal video
// ==========================
window.openVideoModal = async (chapterId, lessonId) => {
    currentLessonEdit = { chapterId: parseInt(chapterId), lessonId: parseInt(lessonId) };

    const titleEl = document.getElementById("videoTitle");
    const videoEl = document.getElementById("videoPreview");
    const freeEl = document.getElementById("editVideoFree");
    const inputEl = document.getElementById("videoUploadInput");

    if (!titleEl || !videoEl || !freeEl || !inputEl) return;

    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

        titleEl.value = lesson.title || "";
        freeEl.checked = !!lesson.isFree;
        inputEl.value = "";

        // ====== Preview video cũ ======
        videoEl.innerHTML = "";
        if (lesson.videoUrl) {
            const source = document.createElement("source");
            source.src = lesson.videoUrl;
            source.type = "video/mp4";
            videoEl.appendChild(source);
            videoEl.load();
        }

        // ====== Preview video khi chọn file mới ======
        inputEl.onchange = () => {
            const file = inputEl.files[0];
            if (!file) return;

            videoEl.innerHTML = "";
            const source = document.createElement("source");
            source.src = URL.createObjectURL(file);
            source.type = file.type;
            videoEl.appendChild(source);
            videoEl.load();
        };

        // Hiển thị modal
        const modalEl = document.getElementById("editVideoModal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        // Reset modal khi đóng
        modalEl.addEventListener("hidden.bs.modal", () => {
            currentLessonEdit = null;
            videoEl.innerHTML = "";
            inputEl.value = "";
        }, { once: true });

    } catch (err) {
        console.error(err);
        Toast.show("Không thể mở modal video!", "danger");
    }
};

// ==========================
// Lưu video + title + isFree
// ==========================
window.saveVideo = async () => {
    if (!currentLessonEdit) return;

    const titleEl = document.getElementById("videoTitle");
    const inputEl = document.getElementById("videoUploadInput");
    const freeEl = document.getElementById("editVideoFree");
    const videoEl = document.getElementById("videoPreview"); // <--- ensure videoEl is defined
    const modalEl = document.getElementById("editVideoModal");
    const modal = bootstrap.Modal.getInstance(modalEl);

    const title = titleEl.value.trim();
    if (!title) {
        Toast.show("Vui lòng nhập tiêu đề video!", "warning");
        return;
    }

    const file = inputEl.files[0];
    const isFree = freeEl.checked;

    try {
        let videoUrl = "";

        if (file) {
            Toast.show("Đang upload video... 0%", "info", 0);
            videoUrl = await uploadVideo(file, (p) => {
                Toast.show(`Đang upload video... ${Math.round(p)}%`, "info", 0);
            });
            Toast.show("Upload video thành công!", "success", 2000);
        } else {
            const lesson = await LessonApi.getLessonById(currentLessonEdit.chapterId, currentLessonEdit.lessonId);
            videoUrl = lesson.videoUrl || "";
        }

        console.log("Gửi lên backend:", {
            chapterId: currentLessonEdit.chapterId,
            lessonId: currentLessonEdit.lessonId,
            title, videoUrl, isFree
        });

        const res = await LessonApi.updateLessonVideo(
            currentLessonEdit.chapterId,
            currentLessonEdit.lessonId,
            { title, videoUrl, isFree }
        );

        console.log("Update response:", res);

        // Cập nhật danh sách lessons
        const lessons = await LessonApi.getLessonsByChapter(currentLessonEdit.chapterId);
        window.lessonListModule?.renderLessonsIntoChapter(currentLessonEdit.chapterId, lessons);

        Toast.show("Cập nhật video thành công!", "success", 2000);

    } catch (err) {
        console.error("Lưu video thất bại:", err);
        Toast.show("Lưu video thất bại!", "danger");
    } finally {
        modal?.hide();
    }
};

// ==========================
// DOMContentLoaded
// ==========================
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("btnSaveVideoLesson")?.addEventListener("click", window.saveVideo);
});
