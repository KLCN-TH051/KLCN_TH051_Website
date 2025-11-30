import AccountApi from "/js/api/AccountApi.js";
import UploadApi from "/js/api/UploadApi.js";
import Toast from "/js/components/Toast.js";

document.addEventListener("DOMContentLoaded", () => {
    initProfile();
});

async function initProfile() {

    // Không có token thì không gọi API
    if (!localStorage.getItem("authToken")) {
        console.warn("⚠️ Không có token! Không thể gọi API profile.");
        return;
    }

    await loadProfile();
    setupAvatarUpload();
}

async function loadProfile() {
    try {
        const res = await AccountApi.getProfile();
        if (!res || !res.success) {
            console.warn("Không lấy được dữ liệu profile:", res);
            return;
        }

        const user = res.data;

        document.getElementById("username").value = user.fullName ?? "";
        document.getElementById("email").value = user.email ?? "";
        document.getElementById("role").value = user.role ?? "Không rõ";

        // ===== FIX lỗi avatar =====
        let avatarUrl = "/images/default-avatar.png";

        if (user.avatar) {
            if (user.avatar.startsWith("http")) {
                avatarUrl = user.avatar;
            } else {
                avatarUrl = UploadApi.getFileUrl("avatar", user.avatar);
            }
        }

        document.getElementById("avatarPreview").src = avatarUrl;

    } catch (err) {
        console.error("Lỗi khi load profile:", err);
    }
}

function setupAvatarUpload() {
    const btnChangeAvatar = document.getElementById("btnChangeAvatar");
    const avatarInput = document.getElementById("avatarInput");
    const avatarPreview = document.getElementById("avatarPreview");

    btnChangeAvatar.addEventListener("click", () => avatarInput.click());

    avatarInput.addEventListener("change", async () => {
        const file = avatarInput.files[0];
        if (!file) return;

        try {
            const uploadRes = await UploadApi.uploadFile(file, "avatar");
            const newUrl = UploadApi.getFileUrl("avatar", uploadRes.fileName);

            avatarPreview.src = newUrl;
            Toast.success("Đổi ảnh đại diện thành công!");
        } catch (err) {
            console.error("Upload avatar lỗi:", err);
            Toast.error("Không thể đổi ảnh đại diện!");
        }
    });
}
