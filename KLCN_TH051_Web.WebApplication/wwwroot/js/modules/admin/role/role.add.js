import RoleApi from "../../../api/RoleApi.js";

const RoleAdd = {
    async init(roleListContainerId = "roleList") {
        this.roleNameInput = document.getElementById("addRoleName");
        this.permissionContainer = document.getElementById("addRolePermissions");
        this.btnAdd = document.getElementById("btnAddRole");
        this.roleListEl = document.getElementById(roleListContainerId);

        if (!this.roleNameInput || !this.permissionContainer || !this.btnAdd) {
            console.warn("Không tìm thấy các phần tử modal thêm role");
            return;
        }

        // Load danh sách permissions để chọn
        await this.loadPermissions();

        // Gắn sự kiện mở modal (nếu có nút mở modal)
        const btnOpen = document.getElementById("btnOpenAddModal");
        if (btnOpen) {
            btnOpen.addEventListener("click", () => {
                const modalEl = document.getElementById("addRoleModal");
                const modal = new bootstrap.Modal(modalEl);
                modal.show();
            });
        }

        // Gắn sự kiện thêm role
        this.btnAdd.addEventListener("click", async () => {
            const roleName = this.roleNameInput.value.trim();
            if (!roleName) {
                alert("Tên vai trò không được để trống");
                return;
            }

            const selected = Array.from(this.permissionContainer.querySelectorAll("input[type=checkbox]:checked"))
                .map(cb => cb.value);

            try {
                // Tạo role + permissions
                await RoleApi.updatePermissions(roleName, selected);

                alert(`Tạo role "${roleName}" thành công`);

                // Close modal
                const modal = bootstrap.Modal.getInstance(document.getElementById("addRoleModal"));
                modal.hide();

                // Clear form
                this.roleNameInput.value = "";
                this.permissionContainer.innerHTML = "";

                // Reload permissions cho modal
                await this.loadPermissions();

                // Update danh sách roles trên UI
                this.addRoleToList(roleName);

            } catch (err) {
                console.error(err);
                alert("Lỗi khi tạo role: " + err.message);
            }
        });
    },

    async loadPermissions() {
        try {
            const permissions = await RoleApi.getClaimValues();
            if (!permissions || permissions.length === 0) {
                this.permissionContainer.innerHTML = `<div class="text-muted">Không có quyền nào</div>`;
                return;
            }

            let html = "";
            permissions.forEach(p => {
                html += `
                    <div class="col-md-3 mb-2">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="${p}" id="perm-${p}">
                            <label class="form-check-label" for="perm-${p}">${p}</label>
                        </div>
                    </div>
                `;
            });

            this.permissionContainer.innerHTML = html;

        } catch (err) {
            console.error(err);
            this.permissionContainer.innerHTML = `<div class="text-danger">Không thể tải danh sách quyền</div>`;
        }
    },

    addRoleToList(roleName) {
        if (!this.roleListEl) return;

        const html = `
            <div class="list-group-item p-3" id="role-${roleName}">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="mb-1">${roleName}</h6>
                        <small class="text-muted">ID: N/A</small>
                    </div>
                    <button class="btn btn-sm btn-outline-primary btn-show-perm" data-name="${roleName}">
                        Xem quyền
                    </button>
                </div>
                <div id="perm-${roleName}" class="mt-2 collapse">
                    <div class="p-2 border rounded bg-light permissions-box">
                        <div class="text-muted small">Đang tải quyền...</div>
                    </div>
                </div>
            </div>
        `;

        // Thêm vào đầu danh sách
        this.roleListEl.insertAdjacentHTML("afterbegin", html);

        // Gắn sự kiện show permission cho role mới
        const btn = document.querySelector(`#role-${roleName} .btn-show-perm`);
        const box = document.getElementById(`perm-${roleName}`);
        btn.addEventListener("click", async () => {
            box.classList.toggle("show");
            if (box.dataset.loaded === "1") return;

            try {
                const permissions = await RoleApi.get(`roles/${roleName}`);
                const boxContent = box.querySelector(".permissions-box");
                if (!permissions || permissions.length === 0) {
                    boxContent.innerHTML = `<div class="text-muted small">Không có quyền nào</div>`;
                    return;
                }

                let htmlPerm = `<div class="d-flex flex-wrap gap-1">`;
                permissions.forEach(p => htmlPerm += `<span class="badge bg-primary">${p}</span>`);
                htmlPerm += `</div>`;

                boxContent.innerHTML = htmlPerm;
                box.dataset.loaded = "1";

            } catch (err) {
                box.querySelector(".permissions-box").innerHTML = `<div class="text-danger">${err.message}</div>`;
            }
        });
    }
};

export default RoleAdd;

// 🚀 AUTO INIT
document.addEventListener("DOMContentLoaded", async () => {
    await RoleAdd.init();
});
