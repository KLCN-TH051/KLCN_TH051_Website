// wwwroot/js/adminlogin.js
document.addEventListener('DOMContentLoaded', () => {
    // FIX CHUẨN NHẤT CHO .NET: dùng querySelector thay currentScript
    const scriptTag = document.querySelector('script[src*="adminlogin.js"]');
    const loginApiUrl = scriptTag?.dataset.apiUrl;

    if (!loginApiUrl) {
        console.error("KHÔNG TÌM THẤY API URL! Kiểm tra thẻ script trong Login.cshtml");
        document.getElementById('loginError').textContent = "Lỗi cấu hình hệ thống";
        document.getElementById('loginError').style.display = 'block';
        return;
    }

    console.log("API URL:", loginApiUrl); // để kiểm tra

    const form = document.getElementById('loginForm');
    if (!form) return;

    form.addEventListener('submit', async (e) => {
        e.preventDefault();
        const email = document.getElementById('email').value.trim();
        const password = document.getElementById('password').value.trim();

        try {
            const response = await fetch(loginApiUrl, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });

            const result = await response.json();
            console.log("Response từ server:", result); // XEM RESPONSE Ở ĐÂY

            if (!response.ok) {
                throw new Error(result.message || result.errors || "Đăng nhập thất bại");
            }

            const token = result.data || result.token;
            if (!token) throw new Error("Không nhận được token");

            const displayName = result.userName || result.fullName || email.split('@')[0] || "User";
            localStorage.setItem('authToken', token);
            localStorage.setItem('userName', displayName);
            localStorage.setItem('userEmail', email);

            console.log("Đăng nhập thành công:", displayName);
            window.location.href = '/Admin/Subject/Index';

        } catch (err) {
            console.error("Lỗi đăng nhập:", err);
            const errDiv = document.getElementById('loginError');
            errDiv.style.display = 'block';
            errDiv.textContent = err.message || "Đã có lỗi xảy ra";
        }
    });
});