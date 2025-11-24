import UserApi from "../../../api/UserApi.js";

console.log(">>> user.del.js LOADED <<<");

let deletingUserId = null;

document.addEventListener("DOMContentLoaded", () => {
    const deleteModalEl = document.getElementById("deleteAccountModal");
    const confirmBtn = document.getElementById("confirmDeleteBtn");
    const deleteNameEl = document.getElementById("deleteAccountName");

    confirmBtn.addEventListener("click", async () => {
        if (!deletingUserId) return;

        try {
            const result = await UserApi.delete(deletingUserId);
            console.log("Delete result:", result);

            const modalInstance = bootstrap.Modal.getInstance(deleteModalEl);
            if (modalInstance) {
                modalInstance.hide();
            }

            document.dispatchEvent(new Event("refreshUserList"));

            alert("Xóa tài khoản thành công!");

        } catch (err) {
            console.error("Lỗi khi xóa user:", err);
            alert("Không thể xóa tài khoản!");
        } finally {
            deletingUserId = null;
            deleteNameEl.textContent = "";
        }
    });
});

window.deleteUser = async (id) => {
    const deleteModalEl = document.getElementById("deleteAccountModal");
    const deleteNameEl = document.getElementById("deleteAccountName");

    try {
        const user = await UserApi.getById(id);

        if (!user) {
            alert("Không tìm thấy tài khoản!");
            return;
        }

        deletingUserId = user.id;
        deleteNameEl.textContent = user.fullName;

        const modalInstance = bootstrap.Modal.getInstance(deleteModalEl) || new bootstrap.Modal(deleteModalEl);
        modalInstance.show();

    } catch (err) {
        console.error("Lỗi load user để xoá:", err);
        alert("Không thể tải dữ liệu người dùng!");
    }
};
