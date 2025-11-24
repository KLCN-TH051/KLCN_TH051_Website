import RoleApi from "../../../api/RoleApi.js";

console.log(">>> user.roles.js LOADED <<<");

document.addEventListener("DOMContentLoaded", async () => {
    await loadRoles();
});

export async function loadRoles() {
    const createRoleSelect = document.getElementById("createRole");
    const editRoleSelect = document.getElementById("editRole");

    if (!createRoleSelect || !editRoleSelect) {
        console.warn("Không tìm thấy select role trong DOM");
        return;
    }

    // Hiển thị loading tạm thời
    createRoleSelect.innerHTML = `<option>Đang tải...</option>`;
    editRoleSelect.innerHTML = `<option>Đang tải...</option>`;

    try {
        const roles = await RoleApi.getAllRoles();
        console.log("Roles tải về:", roles);

        if (!roles || roles.length === 0) {
            createRoleSelect.innerHTML = `<option>Không có role</option>`;
            editRoleSelect.innerHTML = `<option>Không có role</option>`;
            return;
        }

        // Build options
        const html = roles.map(r => `<option value="${r.name}">${r.name}</option>`).join("");

        createRoleSelect.innerHTML = html;
        editRoleSelect.innerHTML = html;

    } catch (err) {
        console.error("Lỗi khi load roles:", err);
        createRoleSelect.innerHTML = `<option>Không thể tải role</option>`;
        editRoleSelect.innerHTML = `<option>Không thể tải role</option>`;
    }
}
