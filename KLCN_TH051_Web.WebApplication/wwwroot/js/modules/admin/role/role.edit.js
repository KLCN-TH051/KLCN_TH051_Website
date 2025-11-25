import RoleApi from "../../../api/RoleApi.js";

const RoleEditPage = {
    modal: null,
    currentRole: null,

    // ================= INIT =================
    async init() {
        this.modalEl = document.getElementById("editRoleModal");
        this.roleNameInput = document.getElementById("editRoleName");
        this.permissionsContainer = document.getElementById("editRolePermissions");
        this.btnSave = document.getElementById("btnSaveRole");

        if (!this.modalEl || !this.roleNameInput || !this.permissionsContainer || !this.btnSave) return;

        this.modal = new bootstrap.Modal(this.modalEl);
        this.btnSave.addEventListener("click", () => this.savePermissions());
    },

    // ================= MỞ MODAL =================
    async open(role) {
        if (!role) return;

        this.currentRole = role;
        this.roleNameInput.value = role.name;

        try {
            const allPermissions = await RoleApi.getClaimValues();
            const rolePermissions = role.permissions || [];

            let html = "";
            allPermissions.forEach(p => {
                const safeId = "perm_" + p.replace(/[^a-zA-Z0-9_-]/g, "_");
                const checked = rolePermissions.includes(p) ? "checked" : "";
                html += `
                    <div class="col-6 col-md-4 mb-2">
                        <div class="form-check">
                            <input class="form-check-input perm-checkbox" type="checkbox" value="${p}" id="${safeId}" ${checked}>
                            <label class="form-check-label small" for="${safeId}">${p}</label>
                        </div>
                    </div>
                `;
            });

            this.permissionsContainer.innerHTML = html;
            this.modal.show();

        } catch (err) {
            console.error("Lỗi khi load permissions:", err);
            this.permissionsContainer.innerHTML = `<div class="text-danger">Không thể load permissions: ${err.message}</div>`;
        }
    },

    // ================= LƯU THAY ĐỔI =================
    async savePermissions() {
        if (!this.currentRole) return;

        const checkedBoxes = this.permissionsContainer.querySelectorAll(".perm-checkbox:checked");
        const selectedPermissions = Array.from(checkedBoxes).map(cb => cb.value);

        try {
            // Lưu permissions, không cần kết quả JSON
            await RoleApi.updatePermissions(this.currentRole.name, selectedPermissions).catch(err => {
                console.warn("Bỏ qua lỗi parse JSON:", err.message);
            });

            this.modal.hide();
            alert("Cập nhật permissions thành công");

            if (window.RoleListPage && typeof window.RoleListPage.loadRoles === "function") {
                window.RoleListPage.loadRoles();
            }
        } catch (err) {
            console.error("Lỗi khi lưu permissions:", err);
            alert("Lỗi khi lưu permissions: " + err.message);
        }
    }
};

// ================= GẮN VÀO WINDOW =================
window.RoleEditPage = RoleEditPage;

// ================= AUTO INIT =================
document.addEventListener("DOMContentLoaded", () => {
    RoleEditPage.init();
});

export default RoleEditPage;
