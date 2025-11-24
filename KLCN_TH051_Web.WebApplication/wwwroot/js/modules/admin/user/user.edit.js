import UserApi from "../../../api/UserApi.js";

console.log(">>> user.edit.js LOADED <<<");

// Biến lưu ID user đang chỉnh sửa
let editingUserId = null;

document.addEventListener("DOMContentLoaded", () => {

    const editForm = document.getElementById("editAccountForm");
    const modalEl = document.getElementById("editAccountModal");
    const editModal = new bootstrap.Modal(modalEl);

    // Submit form chỉnh sửa
    editForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        if (!editingUserId) {
            alert("Không tìm thấy ID user!");
            return;
        }

        const updatedUser = {
            fullName: document.getElementById("editFullName").value.trim(),
            email: document.getElementById("editEmail").value.trim(),
            role: document.getElementById("editRole").value,
            isActive: document.getElementById("editStatus").value === "true",
            password: document.getElementById("editPassword").value || null,
            avatar: null // Tạm thời chưa xử lý đổi ảnh
        };

        try {
            const result = await UserApi.update(editingUserId, updatedUser);
            console.log("Updated:", result);

            alert("Cập nhật thành công!");
            editModal.hide();

            // Reload lại danh sách user
            if (window.loadUsers) window.loadUsers();

        } catch (error) {
            console.error("Lỗi khi cập nhật user:", error);
            alert("Không thể cập nhật tài khoản!");
        }
    });

});


/* ----------------------------------------------------------------
   HÀM MỞ MODAL - ĐƯỢC GỌI TỪ NÚT onclick="editUser(id)"
-----------------------------------------------------------------*/

window.editUser = async (id) => {
    console.log("Edit user:", id);

    const editModal = new bootstrap.Modal(document.getElementById("editAccountModal"));

    try {
        // Lấy thông tin user theo ID
        const user = await UserApi.getById(id);

        if (!user) {
            alert("Không tìm thấy tài khoản!");
            return;
        }

        editingUserId = user.id;

        // Gán dữ liệu vào form
        document.getElementById("editFullName").value = user.fullName;
        document.getElementById("editEmail").value = user.email;
        document.getElementById("editRole").value = user.role;
        document.getElementById("editStatus").value = user.isActive ? "true" : "false";
        document.getElementById("editPassword").value = "";

        // Avatar
        document.getElementById("editAvatar").src =
            user.avatar || "https://placehold.co/100x100";

        // Mở modal
        editModal.show();

    } catch (error) {
        console.error("Lỗi tải user:", error);
        alert("Không thể tải dữ liệu người dùng!");
    }
};
