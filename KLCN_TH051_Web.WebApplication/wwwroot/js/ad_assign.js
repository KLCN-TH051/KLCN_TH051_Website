const API_URL = '@apiUrl/api/Account';
let accounts = [];

function getHeaders() {
    const token = localStorage.getItem('authToken');
    if (!token) {
        window.location.href = '/Admin/Auth/Login';
        return {};
    }
    return { 'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token };
}

async function apiFetch(url, options = {}) {
    const res = await fetch(url, { ...options, headers: getHeaders() });
    if (!res) return null;
    if (res.status === 401 || res.status === 403) {
        localStorage.removeItem('authToken');
        window.location.href = '/Admin/Auth/Login';
        return null;
    }
    return res;
}

async function loadAccounts() {
    const res = await apiFetch(API_URL);
    if (!res) return;
    if (!res.ok) return console.error('Không tải được danh sách tài khoản');
    accounts = await res.json();
    renderAccounts();
}

function renderAccounts() {
    const tbody = document.getElementById('accountTableBody');
    tbody.innerHTML = accounts.length === 0
        ? '<tr><td colspan="8" class="text-center text-muted">Chưa có tài khoản nào</td></tr>'
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
                        <button class="btn btn-sm btn-outline-primary btn-edit"><i class="fa fa-edit"></i></button>
                    </td>
                </tr>
            `;
    });

    document.querySelectorAll('.btn-edit').forEach(btn => {
        btn.addEventListener('click', () => {
            const tr = btn.closest('tr');
            openEditModal(tr.dataset.id);
        });
    });
}

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
        const model = {
            fullName: document.getElementById('editFullName').value,
            email: document.getElementById('editEmail').value,
            role: document.getElementById('editRole').value,
            isActive: document.getElementById('editStatus').value === 'true',
            password: document.getElementById('editPassword').value || null
        };
        const res = await apiFetch(`${API_URL}/${userId}`, { method: 'PUT', body: JSON.stringify(model) });
        if (res && res.ok) {
            alert('Cập nhật tài khoản thành công!');
            modal.hide();
            loadAccounts();
        } else {
            alert('Cập nhật thất bại!');
        }
    };
}

document.addEventListener('DOMContentLoaded', loadAccounts);