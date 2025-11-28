const CART_KEY = "LSS_CART";

export function getCart() {
    return JSON.parse(localStorage.getItem(CART_KEY)) ?? [];
}

export function saveCart(cart) {
    localStorage.setItem(CART_KEY, JSON.stringify(cart));
    updateCartUI();
}

export function addToCart(course) {
    let cart = getCart();

    // Kiểm tra trùng (không thêm 2 lần 1 khóa học)
    if (!cart.some(c => c.id === course.id)) {
        cart.push(course);
        saveCart(cart);
    }
}

export function removeFromCart(id) {
    let cart = getCart().filter(c => c.id !== id);
    saveCart(cart);
}

// ------------------------------
// Update UI Cart tại Header
// ------------------------------
export function updateCartUI() {
    const cart = getCart();

    // Badge số lượng
    document.querySelector("#cart-count").innerText = cart.length;

    // Dropdown content
    const box = document.querySelector("#cart-dropdown-list");
    if (!box) return;

    box.innerHTML = "";

    cart.forEach(c => {
        box.innerHTML += `
            <div class="item">
                <img src="${c.thumbnail}" class="thumb">
                <div class="info">
                    <p class="title">${c.name}</p>
                    <span class="price">${Number(c.price).toLocaleString("vi-VN")}đ</span>
                </div>
            </div>
        `;
    });

    if (cart.length === 0) {
        box.innerHTML = `<p class="text-center text-muted py-2">Giỏ hàng trống</p>`;
    }
}
