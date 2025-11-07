const form = document.getElementById("resetPasswordForm");
if (form) {
    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        const email = document.getElementById("email").value;
        const token = document.getElementById("token").value;
        const newPassword = document.getElementById("newPassword").value;
        const confirmPassword = document.getElementById("confirmPassword").value;
        const submitBtn = document.getElementById("submitBtn");
        const spinner = submitBtn.querySelector(".spinner-border");
        const resultDiv = document.getElementById("resultMessage");
        resultDiv.innerHTML = "";
        submitBtn.disabled = true;
        spinner.classList.remove("d-none");

        if (newPassword !== confirmPassword) {
            resultDiv.innerHTML = `<div class="alert alert-danger">Mật khẩu không khớp!</div>`;
            submitBtn.disabled = false;
            spinner.classList.add("d-none");
            return;
        }

        try {
            const res = await fetch("https://localhost:7134/api/Account/reset-password", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    email: email,
                    token: token,
                    newPassword: newPassword
                })
            });
            const data = await res.json();

            if (res.ok && data.success) {  // ← SỬA: success
                resultDiv.innerHTML = `
                    <div class="alert alert-success">
                        <strong>Thành công!</strong> Đặt lại mật khẩu thành công.
                    </div>`;
                form.reset();
            } else {
                resultDiv.innerHTML = `
                    <div class="alert alert-danger">
                        ${data.message || "Token không hợp lệ hoặc đã hết hạn."} 
                    </div>`;
            }
        } catch (err) {
            resultDiv.innerHTML = `<div class="alert alert-danger">Lỗi kết nối. Vui lòng thử lại.</div>`;
        } finally {
            submitBtn.disabled = false;
            spinner.classList.add("d-none");
        }
    });
}