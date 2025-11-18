/* ========== COMMON LOGIN + FETCH WRAPPER ========== */

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

async function apiFetch(url, options = {}) {
    try {
        const res = await fetch(url, { ...options, headers: getHeaders() });

        if (!res) return null;

        if (res.status === 401 || res.status === 403) {
            localStorage.removeItem('authToken');
            alert('Phiên đăng nhập hết hạn!');
            window.location.href = '/Admin/Auth/Login';
            return null;
        }

        return res;
    } catch (err) {
        console.error("API error:", err);
        return null;
    }
}
