// =============================
// IMPORT
// =============================
import CourseApi from "../../api/courseApi.js";
import { addToCart } from "./cart.icon.js";


// =============================
// AUTO RUN
// =============================
initCourseDetail();


// =============================
// INIT PAGE
// =============================
export async function initCourseDetail() {
    const id = window.COURSE_ID ??
        new URLSearchParams(window.location.search).get("id");

    const container = document.querySelector("#course-detail");
    if (!container) return;

    if (!id) {
        container.innerHTML = `<div class="text-center text-danger py-5">Không tìm thấy khóa học</div>`;
        return;
    }

    container.innerHTML = `<div class="text-center py-5 text-muted">Đang tải dữ liệu...</div>`;

    try {
        const course = await CourseApi.getById(id);
        renderCourseDetail(course);
        bindPreviewEvents();   // bind modal preview
        bindAddToCart(course); // add to cart
    } catch (err) {
        console.error(err);
        container.innerHTML = `<div class="text-center text-danger py-5">Lỗi khi tải khóa học</div>`;
    }
}



// =============================
// RENDER COURSE DETAIL
// =============================
function renderCourseDetail(c) {
    const html = `
        <div class="row mx-auto">

            <!-- LEFT -->
            <div class="col-9">
                <span class="badge bg-primary mb-2">${c.subjectName ?? "Danh mục"}</span>

                <h2 class="fw-bold">${escapeHTML(c.name)}</h2>

                <p>${c.description ?? "Chưa có mô tả"}</p>

                <div class="mb-3">
                    ⭐⭐⭐⭐⭐
                    <div>Giảng viên: <strong>${c.teacherName ?? "Đang cập nhật"}</strong></div>
                </div>

                <!-- Tabs -->
                <ul class="nav nav-tabs mb-3" id="courseTab" role="tablist">
                    <li class="nav-item">
                        <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#content">NỘI DUNG</button>
                    </li>
                    <li class="nav-item">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#teacher">GIẢNG VIÊN</button>
                    </li>
                    <li class="nav-item">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#rating">ĐÁNH GIÁ</button>
                    </li>
                </ul>

                <!-- TAB CONTENT -->
                <div class="tab-content p-3">
                    ${renderTabContent(c)}
                    ${renderTabTeacher(c)}
                    ${renderTabRating()}
                </div>
            </div>

            <!-- RIGHT -->
            <div class="col-3">
                <div class="card p-3 text-center">
                    <img src="${c.thumbnail ?? 'https://placehold.co/300'}"
                         class="img-fluid p-3" style="max-height:180px" />

                    <h4 class="fw-bold">${Number(c.price).toLocaleString("vi-VN")}đ</h4>

                    <button class="btn btn-primary w-100 mt-2 btn-buy">
                        THÊM VÀO GIỎ HÀNG
                    </button>
                </div>

                <div class="card mt-3 p-3">
                    <h5 class="fw-bold">Khóa học bao gồm</h5>
                    <ul class="list-unstyled mt-2">
                        <li><i class="bi bi-play-circle me-2"></i>1 video</li>
                        <li><i class="bi bi-journal-text me-2"></i>3 bài đọc</li>
                        <li><i class="bi bi-question-circle me-2"></i>1 quiz</li>
                        <li>💻 Học mọi nơi</li>
                    </ul>
                </div>
            </div>
        </div>

        <!-- Modal Preview -->
        <div class="modal fade" id="previewModal" tabindex="-1">
            <div class="modal-dialog modal-xl modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Xem trước</h5>
                        <button class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body" id="previewBody" style="min-height:300px;"></div>
                    <div class="modal-footer">
                        <button class="btn btn-light" data-bs-dismiss="modal">Đóng</button>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.querySelector("#course-detail").innerHTML = html;
}



// =============================
// TABS
// =============================
function renderTabContent(c) {
    let chapters = c.chapters ?? [];

    return `
        <div class="tab-pane fade show active" id="content">
            <div class="accordion" id="courseAccordion">
                ${chapters.length === 0
            ? `<div class="text-muted py-4">Chưa có nội dung</div>`
            : chapters.map((ch, i) => `
                        <div class="accordion-item">
                            <h2 class="accordion-header">
                                <button class="accordion-button collapsed"
                                        data-bs-toggle="collapse"
                                        data-bs-target="#c${i}">
                                    Chương ${i + 1}: ${escapeHTML(ch.title ?? "Chưa có tiêu đề")}
                                </button>
                            </h2>

                            <div id="c${i}" class="accordion-collapse collapse">
                                <div class="accordion-body">
                                    <ul class="list-group list-group-flush">
                                        ${renderLessons(ch.lessons)}
                                    </ul>
                                </div>
                            </div>
                        </div>
                    `).join("")
        }
            </div>
        </div>
    `;
}

function renderLessons(lessons = []) {
    return lessons.map(lesson => `
        <li class="list-group-item d-flex justify-content-between align-items-center">
            <div class="d-flex align-items-center">
                ${iconByType(lesson.type)}
                ${escapeHTML(lesson.title)}
            </div>
            <button class="btn btn-outline-success btn-sm btn-preview"
                    data-type="${lesson.type}"
                    data-id="${lesson.id}">
                Xem trước
            </button>
        </li>
    `).join("");
}

function renderTabTeacher(c) {
    return `
        <div class="tab-pane fade text-center p-4" id="teacher">
            <img src="${c.teacherAvatar ?? "https://placehold.co/150"}"
                 class="rounded-circle mb-3"
                 style="width:150px;height:150px;object-fit:cover;">
            <h5 class="fw-bold">${escapeHTML(c.teacherName ?? "Đang cập nhật")}</h5>
        </div>
    `;
}

function renderTabRating() {
    return `
        <div class="tab-pane fade" id="rating">
            <div class="text-center text-muted py-5">Chưa có đánh giá</div>
        </div>
    `;
}

function iconByType(type) {
    switch (type) {
        case "reading": return `<i class="bi bi-journal-text me-2"></i>`;
        case "video": return `<i class="bi bi-play-circle me-2"></i>`;
        case "quiz": return `<i class="bi bi-question-circle me-2"></i>`;
        default: return "";
    }
}



// =============================
// PREVIEW EVENT (Modal)
// =============================
function bindPreviewEvents() {
    const previewBody = document.getElementById("previewBody");
    const modalEl = document.getElementById("previewModal");
    const modal = new bootstrap.Modal(modalEl);

    const dummy = {
        reading: "<h3>Mẫu bài đọc</h3><p>Nội dung sẽ được load từ DB.</p>",
        video: "/videos/sample.mp4",
        quiz: {
            description: "Quiz mẫu",
            questions: [
                { q: "Câu hỏi 1?", answers: ["A", "B"], correct: 0 },
                { q: "Câu hỏi 2?", answers: ["A", "B"], correct: 1 }
            ]
        }
    };

    document.body.addEventListener("click", (e) => {
        const btn = e.target.closest(".btn-preview");
        if (!btn) return;

        const type = btn.dataset.type;

        // Reading
        if (type === "reading") {
            previewBody.innerHTML = dummy.reading;
        }

        // Video
        if (type === "video") {
            previewBody.innerHTML = `
                <video controls preload="metadata" style="width:100%">
                    <source src="${dummy.video}">
                </video>`;
        }

        // Quiz
        if (type === "quiz") {
            let html = `<h5>${dummy.quiz.description}</h5><hr/>`;

            dummy.quiz.questions.forEach((q, i) => {
                html += `
                    <div class="mb-3">
                        <strong>Câu ${i + 1}: ${q.q}</strong>
                        <ul>
                            ${q.answers.map((a, idx) =>
                    `<li ${idx === q.correct ? 'class="text-success fw-bold"' : ''}>${a}</li>`
                ).join("")}
                        </ul>
                    </div>
                `;
            });

            previewBody.innerHTML = html;
        }

        modal.show();
    });
}



// =============================
// ADD TO CART
// =============================
function bindAddToCart(course) {
    const btn = document.querySelector(".btn-buy");
    if (!btn) return;

    btn.addEventListener("click", () => {
        const ok = addToCart({
            id: course.id,
            name: course.name,
            price: course.price,
            thumbnail: course.thumbnail
        });

        alert(ok ? "Đã thêm vào giỏ hàng!" : "Khóa học đã có trong giỏ hàng!");
    });
}



// =============================
// HELPER: XSS SAFE
// =============================
function escapeHTML(str) {
    if (!str) return "";
    return str.replace(/[&<>'"]/g, t => ({
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        "'": '&#39;',
        '"': '&quot;'
    }[t]));
}
