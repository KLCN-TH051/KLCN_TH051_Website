document.addEventListener("DOMContentLoaded", () => {

    // ======== LOGIN FORM =========
    const loginForm = document.getElementById("loginForm");
    if (loginForm) {
        loginForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            const emailInput = document.getElementById("email");
            const passwordInput = document.getElementById("password");
            const rememberMe = document.getElementById("rememberMe")?.checked;

            const email = emailInput.value.trim();
            const password = passwordInput.value.trim();

            const emailError = emailInput.parentElement.querySelector(".error-message");
            const passwordError = passwordInput.parentElement.querySelector(".error-message");

            emailError.style.display = "none";
            passwordError.style.display = "none";
            emailError.textContent = "";
            passwordError.textContent = "";

            let hasError = false;
            if (!email) { emailError.style.display = "block"; emailError.textContent = "Email không được để trống"; hasError = true; }
            else {
                const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailPattern.test(email)) { emailError.style.display = "block"; emailError.textContent = "Email không hợp lệ"; hasError = true; }
            }
            if (!password) { passwordError.style.display = "block"; passwordError.textContent = "Mật khẩu không được để trống"; hasError = true; }
            if (hasError) return;

            const payload = { email, password, rememberMe };

            try {
                const response = await fetch('https://localhost:7134/api/Account/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload)
                });

                const data = await response.json();

                if (response.ok && data.success) {
                    const token = data.data; // JWT token
                    localStorage.setItem("jwtToken", token);

                    // ===== Gọi profile để lấy thông tin user =====
                    const profileRes = await fetch('https://localhost:7134/api/Account/profile', {
                        method: 'GET',
                        headers: {
                            'Authorization': `Bearer ${token}`,
                            'Content-Type': 'application/json'
                        }
                    });
                    const profileData = await profileRes.json();

                    if (profileRes.ok && profileData.success) {
                        localStorage.setItem("userName", profileData.data.fullName);
                        localStorage.setItem("userEmail", profileData.data.email);

                        updateHeaderUser();
                        window.location.href = "/";
                    } else {
                        alert("Không thể lấy thông tin người dùng.");
                    }

                } else {
                    const errorMsg = data?.message || "Email hoặc mật khẩu không đúng";
                    emailError.style.display = "block";
                    passwordError.style.display = "block";
                    emailError.textContent = errorMsg;
                    passwordError.textContent = errorMsg;
                }

            } catch (err) {
                console.error(err);
                alert("Lỗi mạng hoặc server");
            }

        });
    }

    // ======== HEADER USER =========
    function updateHeaderUser() {
        const userName = localStorage.getItem("userName");
        const userEmail = localStorage.getItem("userEmail");

        const userMenu = document.getElementById("userMenu");
        const guestMenu = document.getElementById("guestMenu");
        const userNameSpan = document.getElementById("userName");
        const userAvatar = document.getElementById("userAvatar");

        if (userName && userMenu && guestMenu) {
            userMenu.style.display = "block";
            guestMenu.style.display = "none";
            userNameSpan.textContent = userName;
            userAvatar.src = `https://ui-avatars.com/api/?name=${encodeURIComponent(userName)}&background=0d6efd&color=fff&size=32`;
        } else {
            userMenu.style.display = "none";
            guestMenu.style.display = "block";
        }
    }

    // ======== LOGOUT =========
    const logoutBtn = document.getElementById("logoutBtn");
    if (logoutBtn) {
        logoutBtn.addEventListener("click", (e) => {
            e.preventDefault();
            localStorage.removeItem("jwtToken");
            localStorage.removeItem("userName");
            localStorage.removeItem("userEmail");
            updateHeaderUser();
            window.location.href = "/";
        });
    }

    // Gọi khi load page
    updateHeaderUser();
});
