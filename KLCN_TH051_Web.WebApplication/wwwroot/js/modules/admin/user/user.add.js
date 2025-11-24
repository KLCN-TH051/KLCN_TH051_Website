import UserApi from "../../../api/UserApi.js";

console.log(">>> user.add.js LOADED <<<");

document.addEventListener("DOMContentLoaded", () => {

    const createForm = document.getElementById("createAccountForm");
    const modalEl = document.getElementById("createAccountModal");
    const createModal = new bootstrap.Modal(modalEl);

    // Nhấn nút "+ Thêm" để mở modal
    document.getElementById("btnOpenCreateModal").addEventListener("click", () => {
        resetForm();
        createModal.show();
    });

    // Submit form tạo user
    createForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const newUser = {
            fullName: document.getElementById("createFullName").value.trim(),
            email: document.getElementById("createEmail").value.trim(),
            role: document.getElementById("createRole").value,
            isActive: document.getElementById("createStatus").value === "true",
            password: document.getElementById("createPassword").value,
            avatar: null // chưa làm upload ảnh
        };

        if (!newUser.fullName || !newUser.email || !newUser.password) {
            alert("Vui lòng nhập đầy đủ thông tin!");
            return;
        }

        try {
            const result = await UserApi.create(newUser);
            alert("Tạo tài khoản thành công!");

            createModal.hide();

            // Gửi tín hiệu yêu cầu refresh danh sách
            document.dispatchEvent(new Event("refreshUserList"));

        } catch (error) {
            console.error("Lỗi tạo tài khoản:", error);
            alert("Không thể tạo tài khoản!");
        }

    });

});

/* ---------------------- HÀM HỖ TRỢ ---------------------- */

function resetForm() {
    document.getElementById("createFullName").value = "";
    document.getElementById("createEmail").value = "";
    document.getElementById("createRole").value = "Teacher";
    document.getElementById("createStatus").value = "true";
    document.getElementById("createPassword").value = "";

    document.getElementById("createAvatar").src = "https://placehold.co/100x100";
}
