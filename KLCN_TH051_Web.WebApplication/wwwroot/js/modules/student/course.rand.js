import CourseApi from "../../api/courseApi.js";

// ------------------------------------
// INIT
// ------------------------------------
initRandomCourses();

async function initRandomCourses() {
    await waitForDom();
    await waitForElement("#course-random-carousel");

    const track = document.querySelector("#course-random-carousel");

    if (!track) {
        console.error("Không tìm thấy #course-random-carousel");
        return;
    }

    showLoading(track);

    try {
        let courses = await waitForCoursesWithRetry();

        if (!courses || courses.length === 0) {
            track.innerHTML = `<div class="text-center text-warning py-4 w-100">Đang chờ dữ liệu...</div>`;
            return;
        }

        // ⭐ Chỉ lấy 3 khóa học ngẫu nhiên
        courses = shuffle(courses).slice(0, 3);

        track.innerHTML = "";
        courses.forEach(c => track.innerHTML += courseCard(c));

    } catch (err) {
        console.error("API Error:", err);
        track.innerHTML = `
            <div class="text-center text-danger py-4 w-100">
                Không thể tải khóa học
            </div>
        `;
    }
}

// ------------------------------------
// WAIT FOR DOM READY
// ------------------------------------
function waitForDom() {
    return new Promise(resolve => {
        if (document.readyState === "complete" || document.readyState === "interactive")
            return resolve();
        document.addEventListener("DOMContentLoaded", resolve);
    });
}

// ------------------------------------
// WAIT FOR ELEMENT EXIST
// ------------------------------------
function waitForElement(selector) {
    return new Promise(resolve => {
        if (document.querySelector(selector)) return resolve();

        const observer = new MutationObserver(() => {
            if (document.querySelector(selector)) {
                observer.disconnect();
                resolve();
            }
        });

        observer.observe(document.body, { childList: true, subtree: true });
    });
}

// ------------------------------------
// LOADING SKELETON
// ------------------------------------
function showLoading(track) {
    track.innerHTML = "";

    for (let i = 0; i < 3; i++) {
        track.innerHTML += `
            <div class="carousel-item-course">
                <div class="card h-100 shadow-sm placeholder-glow">
                    <div class="card-img-top placeholder" style="height:180px;"></div>
                    <div class="card-body">
                        <h5 class="card-title placeholder col-6"></h5>
                        <p class="card-text placeholder col-10"></p>
                        <p class="placeholder col-5"></p>
                    </div>
                </div>
            </div>
        `;
    }
}

// ------------------------------------
// FETCH COURSES WITH RETRY
// ------------------------------------
async function waitForCoursesWithRetry(retries = 3, delay = 500) {
    while (retries > 0) {
        try {
            const courses = await CourseApi.getApproved();
            if (Array.isArray(courses) && courses.length > 0) return courses;
        } catch (err) {
            console.warn("API chưa trả dữ liệu, retry...", retries);
        }
        retries--;
        await new Promise(r => setTimeout(r, delay));
    }
    return [];
}

// ------------------------------------
// SHUFFLE
// ------------------------------------
function shuffle(arr) {
    const a = [...arr];
    for (let i = a.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [a[i], a[j]] = [a[j], a[i]];
    }
    return a;
}

// ------------------------------------
// RENDER COURSE CARD
// ------------------------------------
function courseCard(c) {
    const price = Number(c.price ?? 0).toLocaleString("vi-VN");
    const rating = "★★★★☆";

    return `
        <div class="carousel-item-course">
            <div class="card h-100 shadow-sm">
                <img src="${c.thumbnail ?? 'https://placehold.co/600x400'}"
                     class="card-img-top" alt="${c.name}" onerror="this.src='https://placehold.co/600x400'">
                <div class="card-body">
                    <h5 class="card-title">${c.name}</h5>
                    <p class="card-text text-muted">${c.description ?? 'Không có mô tả'}</p>
                    <div class="text-warning mb-2">${rating}</div>
                    <small class="text-muted">GV: ${c.teacherName ?? 'Chưa cập nhật'}</small>
                </div>
                <div class="card-footer bg-white d-flex justify-content-between">
                    <strong>${price}đ</strong>
                    <a href="/Course/Detail/${c.id}" class="btn btn-outline-primary btn-sm">Xem</a>
                </div>
            </div>
        </div>
    `;
}
