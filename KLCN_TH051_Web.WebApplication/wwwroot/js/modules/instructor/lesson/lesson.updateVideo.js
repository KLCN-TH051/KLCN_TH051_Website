////// wwwroot/js/modules/instructor/lesson/lesson.updatevideo.js

////import LessonApi from "../../../api/lessonApi.js";
////import Toast from "../../../components/Toast.js";
////import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
////import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

////// ===================== Firebase Config =====================
////const firebaseConfig = {
////    apiKey: "AIzaSyAPoW_YHRpeV3S5IfU75c2mPZLUVN5h1_o",
////    authDomain: "ho-tro-hoc-tap-1bf1f.firebaseapp.com",
////    projectId: "ho-tro-hoc-tap-1bf1f",
////    storageBucket: "ho-tro-hoc-tap-1bf1f.appspot.com",
////    messagingSenderId: "776893455274",
////    appId: "1:776893455274:web:84536ab780a29d14e7e9d3",
////    measurementId: "G-70838HXZZJ"
////};

////const app = initializeApp(firebaseConfig);
////const storage = getStorage(app);

////let currentLessonEdit = null;

////// ===================== UPLOAD VIDEO =====================
////async function uploadVideo(file) {
////    if (!file) throw "Chưa chọn file video";

////    return new Promise((resolve, reject) => {
////        const storageRef = ref(storage, `lessons/${Date.now()}_${file.name}`);
////        const uploadTask = uploadBytesResumable(storageRef, file);

////        uploadTask.on('state_changed',
////            snapshot => {
////                const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
////                console.log(`Upload is ${progress.toFixed(2)}% done`);
////            },
////            error => reject(error),
////            async () => {
////                try {
////                    const url = await getDownloadURL(uploadTask.snapshot.ref);
////                    resolve(url);
////                } catch (err) {
////                    reject(err);
////                }
////            }
////        );
////    });
////}

////// ===================== MỞ MODAL VIDEO =====================
////window.openVideoModal = async (chapterId, lessonId) => {
////    currentLessonEdit = { chapterId, lessonId };

////    try {
////        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

////        document.getElementById("videoTitle").value = lesson.title || "";
////        document.getElementById("videoPreview").src = lesson.videoUrl || "";
////        const freeEl = document.getElementById("editVideoFree");
////        if (freeEl) freeEl.checked = lesson.isFree || false;
////        document.getElementById("videoUploadInput").value = "";

////        const modalEl = document.getElementById("editVideoModal");
////        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
////        modal.show();

////        // Reset modal khi đóng
////        modalEl.addEventListener("hidden.bs.modal", () => {
////            document.getElementById("videoTitle").value = "";
////            document.getElementById("videoPreview").src = "";
////            if (freeEl) freeEl.checked = false;
////            document.getElementById("videoUploadInput").value = "";
////            currentLessonEdit = null;
////        }, { once: true });

////    } catch (err) {
////        console.error(err);
////        Toast.show("Không tải được thông tin video!", "danger");
////    }
////};

////// ===================== LƯU VIDEO =====================
////window.saveVideo = async () => {
////    if (!currentLessonEdit) return;

////    const title = document.getElementById("videoTitle").value.trim();
////    const fileInput = document.getElementById("videoUploadInput");
////    const file = fileInput.files[0];
////    const isFree = document.getElementById("editVideoFree").checked;

////    if (!title) {
////        Toast.show("Chưa nhập tiêu đề video!", "warning");
////        return;
////    }

////    try {
////        let videoUrl = document.getElementById("videoPreview").src;
////        if (file) videoUrl = await uploadVideo(file);

////        await LessonApi.updateLessonVideo(currentLessonEdit.chapterId, currentLessonEdit.lessonId, {
////            title,
////            videoUrl,
////            isFree
////        });

////        // Reload danh sách bài học
////        const lessons = await LessonApi.getLessonsByChapter(currentLessonEdit.chapterId);
////        window.lessonListModule?.renderLessonsIntoChapter(currentLessonEdit.chapterId, lessons);

////        Toast.show("Cập nhật video thành công!", "success");
////        bootstrap.Modal.getInstance(document.getElementById("editVideoModal"))?.hide();

////    } catch (err) {
////        console.error(err);
////        Toast.show("Cập nhật video thất bại!", "danger");
////    }
////};


//// wwwroot/js/modules/instructor/lesson/lesson.updatevideo.js
//import LessonApi from "../../../api/lessonApi.js";
//import Toast from "../../../components/Toast.js";
//import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
//import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

//// ===================== Firebase Config =====================
//const firebaseConfig = {
//    apiKey: "AIzaSyAPoW_YHRpeV3S5IfU75c2mPZLUVN5h1_o",
//    authDomain: "ho-tro-hoc-tap-1bf1f.firebaseapp.com",
//    projectId: "ho-tro-hoc-tap-1bf1f",
//    storageBucket: "ho-tro-hoc-tap-1bf1f.appspot.com",
//    messagingSenderId: "776893455274",
//    appId: "1:776893455274:web:84536ab780a29d14e7e9d3",
//    measurementId: "G-70838HXZZJ"
//};
//const app = initializeApp(firebaseConfig);
//const storage = getStorage(app);

//let currentLessonEdit = null;

//// ===================== UPLOAD VIDEO + PROGRESS BAR =====================
//async function uploadVideo(file, onProgress) {
//    if (!file) throw new Error("Chưa chọn file video");

//    return new Promise((resolve, reject) => {
//        const fileName = `${Date.now()}_${file.name}`;
//        const storageRef = ref(storage, `lessons/${fileName}`);
//        const uploadTask = uploadBytesResumable(storageRef, file);

//        uploadTask.on('state_changed',
//            (snapshot) => {
//                const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
//                if (onProgress) onProgress(progress, snapshot.state);
//            },
//            (error) => reject(error),
//            async () => {
//                try {
//                    const url = await getDownloadURL(uploadTask.snapshot.ref);
//                    resolve(url);
//                } catch (err) {
//                    reject(err);
//                }
//            }
//        );
//    });
//}

//// ===================== XEM TRƯỚC VIDEO NGAY KHI CHỌN FILE =====================
//function setupVideoPreview() {
//    const input = document.getElementById("videoUploadInput");
//    const preview = document.getElementById("videoPreview");
//    const source = preview.querySelector("source") || document.createElement("source");

//    if (!input || !preview) return;

//    input.addEventListener("change", () => {
//        const file = input.files[0];
//        if (!file) {
//            preview.src = "";
//            source.src = "";
//            return;
//        }

//        // Kiểm tra định dạng video
//        if (!file.type.startsWith("video/")) {
//            Toast.show("Vui lòng chọn file video hợp lệ!", "warning");
//            input.value = "";
//            return;
//        }

//        // Tạo URL tạm để preview ngay lập tức
//        const tempUrl = URL.createObjectURL(file);
//        source.src = tempUrl;
//        source.type = file.type;
//        if (!preview.querySelector("source")) {
//            preview.appendChild(source);
//        }
//        preview.load();
//        preview.play().catch(() => { }); // tự động play (không bắt buộc)

//        // Hiển thị tên file + dung lượng
//        const info = document.getElementById("videoFileInfo");
//        if (info) {
//            info.textContent = `${file.name} (${(file.size / 1024 / 1024).toFixed(2)} MB)`;
//            info.style.display = "block";
//        }
//    });
//}

//// ===================== MỞ MODAL VIDEO =====================
//window.openVideoModal = async (chapterId, lessonId) => {
//    currentLessonEdit = { chapterId, lessonId };

//    try {
//        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

//        // Điền dữ liệu cũ
//        document.getElementById("videoTitle").value = lesson.title || "";
//        const preview = document.getElementById("videoPreview");
//        const source = preview.querySelector("source") || document.createElement("source");
//        source.src = lesson.videoUrl || "";
//        source.type = "video/mp4";
//        if (!preview.querySelector("source")) preview.appendChild(source);
//        preview.load();

//        const freeEl = document.getElementById("editVideoFree");
//        if (freeEl) freeEl.checked = lesson.isFree || false;

//        // Reset input file + info
//        document.getElementById("videoUploadInput").value = "";
//        const info = document.getElementById("videoFileInfo");
//        if (info) info.style.display = "none";

//        // Hiển thị thanh tiến trình (nếu có)
//        const progressContainer = document.getElementById("uploadProgress");
//        if (progressContainer) progressContainer.style.display = "none";

//        const modalEl = document.getElementById("editVideoModal");
//        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
//        modal.show();

//        // Setup preview khi chọn file mới
//        setupVideoPreview();

//        // Reset khi đóng modal
//        modalEl.addEventListener("hidden.bs.modal", () => {
//            preview.src = "";
//            source.src = "";
//            document.getElementById("videoUploadInput").value = "";
//            if (info) info.style.display = "none";
//            currentLessonEdit = null;
//        }, { once: true });

//    } catch (err) {
//        console.error(err);
//        Toast.show("Không tải được thông tin video!", "danger");
//    }
//};

//// ===================== LƯU VIDEO =====================
//window.saveVideo = async () => {
//    if (!currentLessonEdit) return;

//    const title = document.getElementById("videoTitle").value.trim();
//    const fileInput = document.getElementById("videoUploadInput");
//    const file = fileInput.files[0];
//    const isFree = document.getElementById("editVideoFree")?.checked ?? false;

//    if (!title) {
//        Toast.show("Chưa nhập tiêu đề video!", "warning");
//        return;
//    }

//    try {
//        let videoUrl = document.getElementById("videoPreview").querySelector("source")?.src || "";

//        // Nếu có file mới → upload
//        if (file) {
//            const progressContainer = document.getElementById("uploadProgress");
//            const progressBar = document.getElementById("uploadProgressBar");
//            const progressText = document.getElementById("uploadProgressText");

//            if (progressContainer) progressContainer.style.display = "block";

//            videoUrl = await uploadVideo(file, (progress, state) => {
//                if (progressBar) progressBar.style.width = `${progress}%`;
//                if (progressText) {
//                    if (state === "running") {
//                        progressText.textContent = `Đang tải lên... ${progress.toFixed(1)}%`;
//                    } else if (state === "paused") {
//                        progressText.textContent = "Tạm dừng...";
//                    }
//                }
//            });
//        }

//        // Gọi API cập nhật
//        await LessonApi.updateLessonVideo(currentLessonEdit.chapterId, currentLessonEdit.lessonId, {
//            title,
//            videoUrl,
//            isFree
//        });

//        // Cập nhật danh sách bài học
//        const lessons = await LessonApi.getLessonsByChapter(currentLessonEdit.chapterId);
//        window.lessonListModule?.renderLessonsIntoChapter(currentLessonEdit.chapterId, lessons);

//        Toast.show("Cập nhật video thành công!", "success");
//        bootstrap.Modal.getInstance(document.getElementById("editVideoModal"))?.hide();

//    } catch (err) {
//        console.error("Lỗi upload video:", err);
//        Toast.show("Cập nhật video thất bại! " + (err.message || ""), "danger");
//    }
//};

// wwwroot/js/modules/instructor/lesson/lesson.updatevideo.js
import LessonApi from "../../../api/lessonApi.js";
import Toast from "../../../components/Toast.js";
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-app.js";
import { getStorage, ref, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/12.6.0/firebase-storage.js";

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

async function uploadVideo(file, onProgress = null) {
    if (!file) throw new Error("Không có file");
    return new Promise((resolve, reject) => {
        const storageRef = ref(storage, `lessons/${Date.now()}_${file.name}`);
        const uploadTask = uploadBytesResumable(storageRef, file);
        uploadTask.on('state_changed',
            snapshot => {
                const progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
                onProgress?.(progress);
            },
            reject,
            async () => {
                const url = await getDownloadURL(uploadTask.snapshot.ref);
                resolve(url);
            }
        );
    });
}

window.openVideoModal = async (chapterId, lessonId) => {
    currentLessonEdit = { chapterId: parseInt(chapterId), lessonId: parseInt(lessonId) };

    try {
        const lesson = await LessonApi.getLessonById(chapterId, lessonId);

        const titleEl = document.getElementById("videoTitle");
        const videoEl = document.getElementById("videoPreview");
        const freeEl = document.getElementById("editVideoFree");
        const inputEl = document.getElementById("videoUploadInput");

        if (titleEl) titleEl.value = lesson.title || "";
        if (freeEl) freeEl.checked = lesson.isFree || false;
        if (inputEl) inputEl.value = "";

        // Load video cũ
        if (videoEl && lesson.videoUrl) {
            let source = videoEl.querySelector("source");
            if (!source) {
                source = document.createElement("source");
                videoEl.appendChild(source);
            }
            source.src = lesson.videoUrl;
            source.type = "video/mp4";
            videoEl.load();
        }

        // === FIX CHÍNH: Gắn lại listener preview ===
        if (inputEl) {
            inputEl.onchange = null; // Xóa listener cũ
            inputEl.onchange = () => {
                const file = inputEl.files[0];
                if (!file) return;

                const url = URL.createObjectURL(file);
                let source = videoEl.querySelector("source");
                if (!source) {
                    source = document.createElement("source");
                    videoEl.appendChild(source);
                }
                source.src = url;
                source.type = file.type;
                videoEl.load();

                const info = document.getElementById("videoFileInfo");
                if (info) {
                    info.textContent = `${file.name} (${(file.size / 1024 / 1024).toFixed(2)} MB)`;
                    info.style.display = "block";
                }
            };
        }

        const modalEl = document.getElementById("editVideoModal");
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        // === FIX CHÍNH: Chỉ xóa src, không xóa thẻ source ===
        modalEl.addEventListener("hidden.bs.modal", () => {
            currentLessonEdit = null;
            if (videoEl) {
                const source = videoEl.querySelector("source");
                if (source) source.src = "";
                videoEl.load();
            }
            if (inputEl) inputEl.value = "";
        }, { once: true });

    } catch (err) {
        console.error(err);
        Toast.show("Không thể mở video!", "danger");
    }
};

window.saveVideo = async () => {
    if (!currentLessonEdit) return;

    const titleEl = document.getElementById("videoTitle");
    const inputEl = document.getElementById("videoUploadInput");
    const freeEl = document.getElementById("editVideoFree");

    if (!titleEl?.value.trim()) {
        Toast.show("Vui lòng nhập tiêu đề video!", "warning");
        return;
    }

    const title = titleEl.value.trim();
    const file = inputEl?.files[0];
    const isFree = freeEl?.checked ?? false;

    try {
        let videoUrl = document.getElementById("videoPreview")?.querySelector("source")?.src || "";

        if (file) {
            Toast.show("Đang upload video...", "info", 0);
            videoUrl = await uploadVideo(file, (p) => {
                Toast.show(`Đang upload... ${p.toFixed(0)}%`, "info", 0);
            });
        }

        await LessonApi.updateLessonVideo(currentLessonEdit.chapterId, currentLessonEdit.lessonId, {
            title, videoUrl, isFree
        });

        const lessons = await LessonApi.getLessonsByChapter(currentLessonEdit.chapterId);
        window.lessonListModule?.renderLessonsIntoChapter(currentLessonEdit.chapterId, lessons);

        Toast.show("Cập nhật video thành công!", "success");
        bootstrap.Modal.getInstance(document.getElementById("editVideoModal"))?.hide();

    } catch (err) {
        console.error(err);
        Toast.show("Lưu video thất bại!", "danger");
    }
};