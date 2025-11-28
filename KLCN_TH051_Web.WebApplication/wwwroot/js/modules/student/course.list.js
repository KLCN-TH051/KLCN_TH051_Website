import CourseApi from "../../api/courseApi.js";
import CoursePagination from "./course.pagination.js";
import { initFilter } from "./course.filter.js";
import { addToCart } from "./cart.icon.js";

let ALL_COURSES = [];
let pagination;

// =======================================
// INIT
// =======================================
export async function initCourseList() {
    await loadCourses();

    initFilter({
        allCourses: ALL_COURSES,
        pagination
    });
}


// =======================================
// LOAD COURSE
// =======================================
async function loadCourses() {
    const container = document.querySelector("#course-list");
    container.innerHTML = `<div class="col text-center text-muted py-5">Đang tải khóa học...</div>`;

    try {
        ALL_COURSES = await CourseApi.getApproved();

        if (!Array.isArray(ALL_COURSES)) {
            console.error("CourseApi.getApproved trả về không phải mảng:", ALL_COURSES);
            ALL_COURSES = [];
        }

        pagination = new CoursePagination({
            data: ALL_COURSES,
            pageSize: 6,
            onPageChange: (pageItems) => renderCourseList(pageItems)
        });

    } catch (err) {
        console.error("Lỗi loadCourses:", err);
        container.innerHTML = `<div class="col text-center text-danger py-5">Không thể tải danh sách khóa học</div>`;
    }
}


// =======================================
// RENDER UI
// =======================================
function renderCourseList(list) {
    const container = document.querySelector("#course-list");
    container.innerHTML = "";

    if (!list || list.length === 0) {
        container.innerHTML = `<div class="col text-center text-muted py-5">Không có khóa học</div>`;
        return;
    }

    list.forEach(c => {
        container.innerHTML += renderCourseCard(c);
    });

    // ---------------------------------------
    // GẮN EVENT ADD TO CART
    // ---------------------------------------
    container.querySelectorAll(".btn-add-cart").forEach(btn => {
        btn.onclick = () => {
            const id = btn.dataset.id;
            const course = ALL_COURSES.find(x => x.id == id);
            if (!course) return;

            addToCart({
                id: course.id,
                name: course.name,
                price: course.price,
                thumbnail: course.thumbnail ?? "https://placehold.co/100x60"
            });

            alert("Đã thêm vào giỏ hàng!");
        };
    });
}


// =======================================
// CARD TEMPLATE
// =======================================
function renderCourseCard(c) {
    const price = Number(c.price ?? 0).toLocaleString("vi-VN");
    const rating = "★★★★☆";

    return `
        <div class="col">
            <div class="card h-100 shadow-sm">

                <img src="${c.thumbnail ?? 'https://placehold.co/500x250'}"
                     class="card-img-top"
                     alt="${c.name}">

                <div class="card-body">
                    <h5 class="card-title">${c.name}</h5>
                    <p class="card-text">${c.description ?? 'Không có mô tả'}</p>
                    <div class="mb-2 text-warning">${rating}</div>
                    <small class="text-muted">GV: ${c.teacherName ?? 'Chưa cập nhật'}</small>
                </div>

                <div class="card-footer bg-white d-flex justify-content-between">
                    <strong>${price}đ</strong>
                    <button class="btn btn-outline-primary btn-sm btn-add-cart" data-id="${c.id}">
                        🛒
                    </button>
                </div>

            </div>
        </div>
    `;
}


// =======================================
// AUTO RUN
// =======================================
initCourseList();
