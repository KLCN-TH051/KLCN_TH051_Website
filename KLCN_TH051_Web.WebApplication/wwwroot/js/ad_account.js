const API_URL = document.querySelector("body").dataset.apiAccount;
let accounts = [];

// Lấy token
function getHeaders() {
    const token = localStorage.getItem('authToken');
    if (!token) {
        alert('Phiên đăng nhập hết hạn!');
        window.location.href = '/Admin/Auth/Login';
        return {};
    }
    return {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token
    };
}

// Fetch wrapper
async function apiFetch(url, options = {}) {
    const res = await fetch(url, { ...options, headers: getHeaders() });
    if (!res) return null;

    if (res.status === 401 || res.status === 403) {
        localStorage.removeItem('authToken');
        alert('Phiên đăng nhập hết hạn!');
        window.location.href = '/Admin/Auth/Login';
        return null;
    }
    return res;
}

// Load danh sách tài khoản
async function loadAccounts() {
    try {
        const res = await apiFetch(API_URL);
        if (!res) return;

        if (!res.ok) {
            console.error("Không tải được danh sách tài khoản");
            return;
        }

        accounts = await res.json();
        renderAccounts();

    } catch (err) {
        console.error(err);
    }
}

// Render bảng tài khoản
function renderAccounts() {
    const tbody = document.getElementById('accountTableBody');
    tbody.innerHTML = accounts.length === 0
        ? `<tr><td colspan="8" class="text-center text-muted">Chưa có tài khoản nào</td></tr>`
        : '';

    accounts.forEach((user, index) => {
        tbody.innerHTML += `
            <tr data-id="${user.id}">
                <td>${index + 1}</td>
                <td><img src="${user.avatar || 'https://placehold.co/40x40'}" class="rounded-circle" /></td>
                <td>${user.fullName}</td>
                <td>${user.email}</td>
                <td><span class="badge ${user.role === 'Admin' ? 'bg-danger' : user.role === 'Giảng viên' ? 'bg-info' : 'bg-secondary'}">${user.role}</span></td>
                <td>${new Date(user.createdDate).toLocaleDateString()}</td>
                <td><span class="badge ${user.isActive ? 'bg-success' : 'bg-danger'}">${user.isActive ? 'Hoạt động' : 'Tạm khóa'}</span></td>
                <td class="text-end">
                    <button class="btn btn-sm btn-outline-primary btn-edit">
                        <i class="fa fa-edit"></i>
                    </button>
                </td>
            </tr>
        `;
    });

    document.querySelectorAll('.btn-edit').forEach(btn => {
        btn.onclick = () => openEditModal(btn.closest("tr").dataset.id);
    });
}

// Mở modal chỉnh sửa
function openEditModal(userId) {
    const user = accounts.find(u => u.id == userId);
    if (!user) return;

    document.getElementById('editFullName').value = user.fullName;
    document.getElementById('editEmail').value = user.email;
    document.getElementById('editPassword').value = '';
    document.getElementById('editRole').value = user.role;
    document.getElementById('editStatus').value = user.isActive ? 'true' : 'false';
    document.getElementById('editAvatar').src = user.avatar || 'https://placehold.co/100x100';

    const modal = new bootstrap.Modal(document.getElementById('editAccountModal'));
    modal.show();

    document.getElementById('editAccountForm').onsubmit = async e => {
        e.preventDefault();
        await submitEditAccount(userId, modal);
    };
}

// Submit update
async function submitEditAccount(userId, modal) {
    const model = {
        fullName: document.getElementById('editFullName').value,
        email: document.getElementById('editEmail').value,
        role: document.getElementById('editRole').value,
        isActive: document.getElementById('editStatus').value === 'true',
        password: document.getElementById('editPassword').value || null
    };

    const res = await apiFetch(`${API_URL}/${userId}`, {
        method: 'PUT',
        body: JSON.stringify(model)
    });

    if (!res) return;

    if (res.ok) {
        alert('Cập nhật tài khoản thành công!');
        modal.hide();
        loadAccounts();
    } else {
        alert('Cập nhật thất bại!');
    }
}

// Auto load
document.addEventListener("DOMContentLoaded", loadAccounts);

let ALL_PERMISSIONS = [];

const API_ROLES = document.body.dataset.apiRoles;
const API_CLAIMS = document.body.dataset.apiClaims;


function getHeaders() {
    const token = localStorage.getItem('authToken');
    if (!token) {
        alert("Phiên đăng nhập hết hạn!");
        window.location.href = '/Admin/Auth/Login';
        return {};
    }
    return {
        "Content-Type": "application/json",
        "Authorization": "Bearer " + token
    };
}

async function apiFetch(url, options = {}) {
    try {
        const res = await fetch(url, { ...options, headers: getHeaders() });
        if (!res) return null;

        if (res.status === 401 || res.status === 403) {
            localStorage.removeItem('authToken');
            alert("Phiên đăng nhập hết hạn!");
            window.location.href = '/Admin/Auth/Login';
            return null;
        }

        return res;

    } catch (err) {
        console.error("API error:", err);
        return null;
    }
}


// ==================== LOAD PERMISSIONS ====================

async function loadAllPermissions() {
    const res = await apiFetch(API_CLAIMS);
    if (!res || !res.ok) return;

    ALL_PERMISSIONS = await res.json();
}


// ==================== RENDER PERMISSION CHECKBOXES ====================

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



// ==================== LOAD ROLES ====================

async function loadRoles() {
    const res = await apiFetch(API_ROLES);
    if (!res || !res.ok) return;

    const roles = await res.json();
    const listGroup = document.getElementById("roleList");
    listGroup.innerHTML = "";

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

        item.addEventListener("click", () => openEditRole(role));

        listGroup.appendChild(item);
    });
}



// ==================== OPEN EDIT MODAL ====================

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



// ==================== ADD ROLE ====================

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
        const txt = await res.text();
        alert("Thêm thất bại: " + txt);
    }
});



// ==================== SAVE ROLE ====================

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
        const txt = await res.text();
        alert("Cập nhật thất bại: " + txt);
    }
});



// ==================== INIT ADD ROLE MODAL ====================

document.getElementById("addRoleModal").addEventListener("show.bs.modal", async () => {
    await loadAllPermissions();
    document.getElementById("addRoleName").value = "";
    document.getElementById("addRoleDesc").value = "";
    renderPermissions("addRolePermissions", []);
});



// ==================== AUTO LOAD ====================

document.addEventListener("DOMContentLoaded", loadRoles);
