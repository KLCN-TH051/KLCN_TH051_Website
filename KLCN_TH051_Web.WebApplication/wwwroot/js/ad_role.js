/* ===================== ROLE MANAGEMENT ======================== */

const API_ROLES = document.body.dataset.apiRoles;
const API_CLAIMS = document.body.dataset.apiClaims;

let ALL_PERMISSIONS = [];

// Load permissions
async function loadAllPermissions() {
    const res = await apiFetch(API_CLAIMS);
    if (!res || !res.ok) return;
    ALL_PERMISSIONS = await res.json();
}

// Render checkbox quyền
function renderPermissions(containerId, selected = []) {
    const container = document.getElementById(containerId);
    container.innerHTML = "";

    ALL_PERMISSIONS.forEach(p => {
        container.innerHTML += `
            <div class="col-md-4 mb-1">
                <input type="checkbox" value="${p}" ${selected.includes(p) ? "checked" : ""}>
                ${p}
            </div>
        `;
    });
}

// Load danh sách role
async function loadRoles() {
    const res = await apiFetch(API_ROLES);
    if (!res || !res.ok) return;

    const roles = await res.json();
    const list = document.getElementById("roleList");
    list.innerHTML = "";

    roles.forEach(role => {
        const name = role.name || role.Name;
        const perms = role.permissions || role.Permissions || [];

        const item = document.createElement("a");
        item.className = "list-group-item list-group-item-action d-flex justify-content-between";
        item.href = "#";
        item.dataset.roleName = name;

        item.innerHTML = `
            <div>
                <div class="fw-semibold">${name}</div>
                <small class="text-muted">(${perms.length}) quyền</small>
            </div>
            <i class="fa fa-chevron-right"></i>
        `;

        item.onclick = () => openEditRole(role);

        list.appendChild(item);
    });
}

// Mở modal edit role
async function openEditRole(role) {
    await loadAllPermissions();

    const name = role.name || role.Name;
    const desc = role.description || role.Description || "";
    const perms = role.permissions || role.Permissions || [];

    document.getElementById("editRoleName").value = name;
    document.getElementById("editRoleDesc").value = desc;

    renderPermissions("editRolePermissions", perms);

    new bootstrap.Modal(document.getElementById("editRoleModal")).show();
}

// Thêm vai trò
document.getElementById("btnAddRole").addEventListener("click", async () => {
    const name = document.getElementById("addRoleName").value.trim();
    const desc = document.getElementById("addRoleDesc").value.trim();

    if (!name) return alert("Bạn phải nhập tên vai trò!");

    const permissions = Array.from(document.querySelectorAll("#addRolePermissions input:checked"))
        .map(cb => cb.value);

    const res = await apiFetch(API_ROLES, {
        method: "POST",
        body: JSON.stringify({
            Name: name,
            Description: desc,
            Permissions: permissions
        })
    });

    if (!res) return;

    if (res.ok) {
        alert("Thêm vai trò thành công!");
        bootstrap.Modal.getInstance(document.getElementById("addRoleModal")).hide();
        loadRoles();
    } else {
        alert("Thêm thất bại!");
    }
});

// Save vai trò
document.getElementById("btnSaveRole").addEventListener("click", async () => {
    const name = document.getElementById("editRoleName").value;

    const permissions = Array.from(document.querySelectorAll("#editRolePermissions input:checked"))
        .map(cb => cb.value);

    const res = await apiFetch(`${API_ROLES}/${name}/permissions`, {
        method: "PUT",
        body: JSON.stringify(permissions)
    });

    if (!res) return;

    if (res.ok) {
        alert("Cập nhật thành công!");
        bootstrap.Modal.getInstance(document.getElementById("editRoleModal")).hide();
        loadRoles();
    } else {
        alert("Cập nhật thất bại!");
    }
});

// Init khi mở modal Add Role
document.getElementById("addRoleModal").addEventListener("show.bs.modal", async () => {
    await loadAllPermissions();
    document.getElementById("addRoleName").value = "";
    document.getElementById("addRoleDesc").value = "";
    renderPermissions("addRolePermissions", []);
});

document.addEventListener("DOMContentLoaded", loadRoles);
