import UserApi from "../../../api/UserApi.js";

console.log(">>> user.list.js LOADED <<<");

document.addEventListener("DOMContentLoaded", () => {
    loadUsers();
});

// Hàm load danh sách user
async function loadUsers() {
    const tableBody = document.getElementById("accountTableBody");

    tableBody.innerHTML = `
        <tr>
            <td colspan="8" class="text-center py-3">Đang tải dữ liệu...</td>
        </tr>
    `;

    try {
        // Fetch API trả về JSON trực tiếp, không có .data
        const users = await UserApi.getAll();
        console.log("Users:", users);

        if (!users || users.length === 0) {
            tableBody.innerHTML = `
                <tr>
                    <td colspan="8" class="text-center py-3">Không có dữ liệu</td>
                </tr>
            `;
            return;
        }

        let html = "";

        users.forEach(user => {
            html += `
                <tr>
                    <td>${user.id}</td>

                    <td>
                        <img src="${user.avatar || "/assets/img/no-avatar.png"}"
                             alt="avatar"
                             class="rounded-circle"
                             width="45" height="45">
                    </td>

                    <td>${user.fullName}</td>
                    <td>${user.email}</td>
                    <td>${user.role}</td>

                    <td>${formatDate(user.createdDate)}</td>

                    <td>
                        ${user.isActive
                    ? `<span class="badge bg-success">Hoạt động</span>`
                    : `<span class="badge bg-secondary">Khóa</span>`}
                    </td>

                    <td class="text-end">
                        <button class="btn btn-sm btn-primary me-1" onclick="editUser(${user.id})">
                            <i class="bi bi-pencil"></i>
                        </button>

                        <button class="btn btn-sm btn-danger" onclick="deleteUser(${user.id})">
                            <i class="bi bi-trash"></i>
                        </button>
                    </td>
                </tr>
            `;
        });

        tableBody.innerHTML = html;

    } catch (error) {
        console.error("Lỗi khi tải users:", error);
        tableBody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-danger py-3">
                    Không thể tải dữ liệu!
                </td>
            </tr>
        `;
    }
}

// Format ngày
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString("vi-VN");
}

// Dummy functions để tránh lỗi khi nhấn nút
window.editUser = (id) => {
    alert("Chỉnh sửa user ID: " + id);
};

window.deleteUser = (id) => {
    if (confirm("Bạn có chắc muốn xóa user này?")) {
        console.log("Xóa user ID:", id);
    }
};
