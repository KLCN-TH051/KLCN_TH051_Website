import { getCart, removeFromCart, updateCartUI } from "./cart.icon.js";

/* ========================= RENDER CART PAGE ========================= */

function renderCartPage() {
    const cart = getCart();
    const list = document.querySelector("#cart-page-list");
    const summaryList = document.querySelector("#summary-list");
    const totalBox = document.querySelector("#summary-total");

    list.innerHTML = "";
    summaryList.innerHTML = "";

    let total = 0;

    if (cart.length === 0) {
        list.innerHTML = `
            <li class="list-group-item text-center text-muted py-4">
                Giỏ hàng trống
            </li>`;
        totalBox.innerText = "0đ";
        return;
    }

    cart.forEach(c => {
        const price = Number(c.price);
        total += price;

        /* --- LEFT LIST (Main) --- */
        list.innerHTML += `
            <li class="list-group-item">
                <div class="row align-items-center">

                    <div class="col-7 d-flex align-items-center">
                        <img src="${c.thumbnail}" class="me-3 rounded"
                             style="width:110px;height:60px;">
                        <span class="fw-semibold">${c.name}</span>
                    </div>

                    <div class="col-3 fw-semibold">
                        ${price.toLocaleString("vi-VN")}đ
                    </div>

                    <div class="col-2 text-end">
                        <button class="btn btn-light border-0 p-1 cart-remove"
                                data-id="${c.id}">
                            <i class="bi bi-trash fs-5 text-danger"></i>
                        </button>
                    </div>

                </div>
            </li>
        `;

        /* --- RIGHT SUMMARY --- */
        summaryList.innerHTML += `
            <div class="d-flex justify-content-between">
                <span>${c.name}</span>
                <span>${price.toLocaleString("vi-VN")}đ</span>
            </div>
        `;
    });

    totalBox.innerText = total.toLocaleString("vi-VN") + "đ";

    attachRemoveEvents();
}

/* ========================= REMOVE ITEM ========================= */

function attachRemoveEvents() {
    document.querySelectorAll(".cart-remove").forEach(btn => {
        btn.addEventListener("click", () => {
            const id = btn.dataset.id;
            removeFromCart(id);   // xoá localStorage
            updateCartUI();       // cập nhật header
            renderCartPage();     // reload trang cart
        });
    });
}

/* ========================= INIT ========================= */
renderCartPage();
updateCartUI();
