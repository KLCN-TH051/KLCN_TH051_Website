// course.utils.js

// ------------------------------
// Format trạng thái
// ------------------------------
export function getStatusBadge(status) {
    const map = {
        0: "Chờ duyệt",
        1: "Đã duyệt",
        2: "Từ chối",
        3: "Bản nháp"
    };

    const color = {
        0: "warning",
        1: "success",
        2: "danger",
        3: "secondary"
    };

    return `<span class="badge bg-${color[status] || "secondary"}">${map[status] || "Không rõ"}</span>`;
}

// ------------------------------
// Format giá
// ------------------------------
export function formatPrice(num) {
    return num ? num.toLocaleString("vi-VN") + "đ" : "0đ";
}
