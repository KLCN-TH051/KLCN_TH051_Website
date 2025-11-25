import RoleApi from "../../../api/RoleApi.js";

const RoleListPage = {

    async init() {
        console.log(">>> role.list.js INIT <<<");
        await this.loadRoles();
    },

    // ================= LOAD DANH SÁCH ROLE =================
    async loadRoles() {
        const roleListEl = document.getElementById("roleList");
        if (!roleListEl) return;

        roleListEl.innerHTML = `<div class="text-center py-3 text-muted">Đang tải...</div>`;

        try {
            const roles = await RoleApi.getAll();
            if (!roles || roles.length === 0) {
                roleListEl.innerHTML = `<div class="text-center py-3 text-muted">Không có vai trò nào</div>`;
                return;
            }

            let html = "";
            for (const role of roles) {
                html += `
                    <div class="list-group-item p-3">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6 class="mb-1">${role.name}</h6>
                                <small class="text-muted">ID: ${role.id}</small>
                            </div>
                            <div>
                                <button class="btn btn-sm btn-outline-warning btn-edit-role"
                                    data-role='${JSON.stringify(role)}'>
                                    <i class="fa fa-edit me-1"></i> Sửa quyền
                                </button>
                            </div>
                        </div>
                    </div>
                `;
            }

            roleListEl.innerHTML = html;
            this.registerEditButtons();

        } catch (err) {
            console.error(err);
            roleListEl.innerHTML = `<div class="text-danger text-center py-3">${err.message}</div>`;
        }
    },

    // ================= NÚT SỬA =================
    registerEditButtons() {
        const buttons = document.querySelectorAll(".btn-edit-role");
        buttons.forEach(btn => {
            btn.addEventListener("click", () => {
                const role = JSON.parse(btn.dataset.role);
                if (window.RoleEditPage) {
                    window.RoleEditPage.open(role); // truyền role object đầy đủ
                } else {
                    console.warn("RoleEditPage chưa được load!");
                }
            });
        });
    }
};

export default RoleListPage;

// ================= AUTO INIT =================
document.addEventListener("DOMContentLoaded", () => {
    RoleListPage.init();
    window.RoleListPage = RoleListPage; // gắn vào window để RoleEditPage có thể gọi loadRoles()
});
