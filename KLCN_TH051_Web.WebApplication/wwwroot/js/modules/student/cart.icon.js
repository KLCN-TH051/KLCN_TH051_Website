import UploadApi from "../../api/uploadApi.js";   // ⭐ THÊM DÒNG NÀY

const CART_KEY = "LSS_CART";

/* ================================
   UTIL
================================ */
export function getCart() {
    try {
        return JSON.parse(localStorage.getItem(CART_KEY)) ?? [];
    } catch {
        return [];
    }
}

export function saveCart(cart) {
    localStorage.setItem(CART_KEY, JSON.stringify(cart));
    updateCartUI();
}

/* ================================
   CART ACTIONS
================================ */
export function addToCart(course) {
    const cart = getCart();

    // Không thêm trùng
    if (cart.some(c => c.id === course.id)) return false;

    cart.push(course);
    saveCart(cart);
    return true;
}

export function removeFromCart(id) {
    const cart = getCart().filter(c => c.id != id);
    saveCart(cart);
}

/* ================================
   RENDER HEADER CART UI
================================ */
export function updateCartUI() {
    const cart = getCart();

    /** ---- Update số lượng ---- **/
    const countEl = document.querySelector("#cart-count");
    if (countEl) countEl.innerText = cart.length;

    /** ---- Update dropdown cart ---- **/
    const box = document.querySelector("#cart-dropdown-list");
    if (!box) return;

    if (cart.length === 0) {
        box.innerHTML = `<p class="text-center text-muted py-2">Giỏ hàng trống</p>`;
        return;
    }

    box.innerHTML = cart.map(c => {

        // ⭐ Build URL thumbnail từ UploadApi
        const thumbnail = UploadApi.getFileUrl("course", c.thumbnail)
            || "https://placehold.co/60x60?text=No+Image";

        return `
            <div class="cart-item" data-id="${c.id}">
                <img src="${thumbnail}" 
                     onerror="this.src='https://placehold.co/60x60?text=Error'" 
                     class="thumb">

                <div class="info">
                    <p class="title">${c.name}</p>
                    <span class="price">
                        ${Number(c.price).toLocaleString("vi-VN")}đ
                    </span>
                </div>

                <button class="dropdown-remove" data-id="${c.id}">
                    <i class="bi bi-x-lg"></i>
                </button>
            </div>
        `;
    }).join("");
}

/* ================================
   EVENT DELEGATION FOR REMOVE
================================ */
document.addEventListener("click", (e) => {
    const btn = e.target.closest(".dropdown-remove");

    if (btn) {
        removeFromCart(btn.dataset.id);
    }
});

/* ================================
   TOGGLE CART DROPDOWN
================================ */
document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector("#cart-toggle");
    const dropdown = document.querySelector(".cart-dropdown");

    if (!toggle || !dropdown) return;

    toggle.addEventListener("click", (e) => {
        e.preventDefault();
        dropdown.classList.toggle("show");
    });

    document.addEventListener("click", (e) => {
        if (!dropdown.contains(e.target) && !toggle.contains(e.target)) {
            dropdown.classList.remove("show");
        }
    });
});
