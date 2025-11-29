// =============================
// IMPORT
// =============================
import CourseApi from "../../api/courseApi.js";
import UploadApi from "../../api/uploadApi.js";   // ⭐ THÊM DÒNG NÀY
import CoursePagination from "./course.pagination.js";
import { initFilter } from "./course.filter.js";
import { addToCart } from "./cart.icon.js";


// =============================
// STATE
// =============================
let ALL_COURSES = [];
let pagination = null;

const container = document.querySelector("#course-list");
if (!container) console.warn("Không tìm thấy #course-list");


// =============================
// INIT
// =============================
export async function initCourseList() {
    await loadCourses();
    initFilter({ allCourses: ALL_COURSES, pagination });
}


// =============================
// LOAD COURSES
// =============================
async function loadCourses() {
    container.innerHTML = loadingHTML();

    try {
        const data = await CourseApi.getApproved();
        ALL_COURSES = Array.isArray(data) ? data : [];

        pagination = new CoursePagination({
            data: ALL_COURSES,
            pageSize: 6,
            onPageChange: renderCourseList
        });

    } catch (err) {
        console.error("Lỗi loadCourses:", err);
        container.innerHTML = errorHTML("Không thể tải danh sách khóa học");
    }
}


// =============================
// RENDER LIST
// =============================
function renderCourseList(list = []) {
    if (!list.length) {
        container.innerHTML = emptyHTML("Không có khóa học");
        return;
    }

    container.innerHTML = list.map(renderCourseCard).join("");
}


// =============================
// EVENT: Delegation
// =============================
document.addEventListener("click", (e) => {

    // --- ADD TO CART ---
    const btn = e.target.closest(".btn-add-cart");
    if (btn) return handleAddToCart(btn);

    // --- CARD CLICK ---
    const card = e.target.closest(".course-card");
    if (card && !e.target.closest(".btn-add-cart")) {
        return handleCardRedirect(card);
    }
});


// =============================
// ADD TO CART
// =============================
function handleAddToCart(btn) {
    const id = btn.dataset.id;
    const course = ALL_COURSES.find(x => x.id == id);
    if (!course) return;

    // ⭐ Lấy URL thumbnail đúng
    const thumbnail = UploadApi.getFileUrl("course", course.thumbnail)
        || "https://placehold.co/100x60?text=No+Image";

    const ok = addToCart({
        id: course.id,
        name: course.name,
        price: course.price,
        thumbnail: thumbnail
    });

    alert(ok ? "Đã thêm vào giỏ hàng!" : "Khóa học đã có trong giỏ hàng!");
}


// =============================
// CARD REDIRECT
// =============================
function handleCardRedirect(card) {
    const id = card.dataset.id;
    if (!id) return;

    window.location.href = `/Course/Detail/${id}`;
}


// =============================
// RENDER CARD
// =============================
function renderCourseCard(c) {
    const price = Number(c.price ?? 0).toLocaleString("vi-VN");

    // ⭐ Lấy URL thumbnail từ API
    const thumbnail = UploadApi.getFileUrl("course", c.thumbnail)
        || "https://placehold.co/500x250?text=No+Image";

    return `
        <div class="col">
            <div class="card h-100 shadow-sm course-card" data-id="${c.id}" style="cursor:pointer;">
                
                <img src="${thumbnail}" 
                     onerror="this.src='https://placehold.co/500x250?text=Error'"
                     class="card-img-top" 
                     alt="${escapeHTML(c.name)}">

                <div class="card-body">
                    <h5 class="card-title">${escapeHTML(c.name)}</h5>

                    <p class="card-text small text-muted" style="min-height:40px;">
                        ${limitText(c.description ?? "Không có mô tả", 80)}
                    </p>

                    <div class="mb-2 text-warning">★★★★☆</div>
                    <small class="text-muted">GV: ${escapeHTML(c.teacherName ?? "Chưa cập nhật")}</small>
                </div>

                <div class="card-footer bg-white d-flex justify-content-between align-items-center">
                    <strong>${price}đ</strong>
                    <button class="btn btn-outline-primary btn-sm btn-add-cart" data-id="${c.id}">
                        🛒
                    </button>
                </div>

            </div>
        </div>
    `;
}


// =============================
// SMALL UTIL HTML
// =============================
const loadingHTML = () =>
    `<div class="col text-center text-muted py-5">Đang tải khóa học...</div>`;

const emptyHTML = (msg) =>
    `<div class="col text-center text-muted py-5">${msg}</div>`;

const errorHTML = (msg) =>
    `<div class="col text-center text-danger py-5">${msg}</div>`;


// =============================
// HELPER — Safe & Format
// =============================
function escapeHTML(str = "") {
    return str.replace(/[&<>'"]/g, t => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        "'": "&#39;",
        '"': "&quot;"
    }[t]));
}

function limitText(str, max) {
    return str.length > max ? str.slice(0, max) + "..." : str;
}


// =============================
// AUTO RUN
// =============================
initCourseList();
