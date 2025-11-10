const API_URL = "https://localhost:7134/api/Account/register";

// Input elements
const fullNameInput = document.getElementById("fullName");
const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");
const confirmPasswordInput = document.getElementById("confirmPassword");
const phoneNumberInput = document.getElementById("phoneNumber");
const dateOfBirthInput = document.getElementById("dateOfBirth");
const messageBox = document.getElementById("registerMessage");
const registerBtn = document.getElementById("registerBtn");
const btnText = document.getElementById("btnText");
const btnLoading = document.getElementById("btnLoading");

// Submit form
document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const data = {
        fullName: fullNameInput.value.trim(),
        email: emailInput.value.trim(),
        password: passwordInput.value.trim(),
        confirmPassword: confirmPasswordInput.value.trim(), // nếu backend không cần thì xóa
        phoneNumber: phoneNumberInput.value.trim(),
        dateOfBirth: dateOfBirthInput.value
    };

    // Disable button & show loading
    registerBtn.disabled = true;
    btnText.classList.add("d-none");
    btnLoading.classList.remove("d-none");

    try {
        const res = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        const json = await res.json();

        if (json.success) {
            showMessage("✅ Đăng ký thành công! Vui lòng kiểm tra email để xác thực.", true);
            e.target.reset();
        } else {
            let errMsg = "Đăng ký thất bại.";
            if (json.errors) {
                if (Array.isArray(json.errors)) {
                    errMsg = json.errors.join("<br>");
                } else {
                    errMsg = Object.values(json.errors).flat().join("<br>");
                }
            } else if (json.message) {
                errMsg = json.message;
            }
            showMessage("⚠️ " + errMsg, false);
        }

    } catch (err) {
        showMessage("❌ Không thể kết nối máy chủ!", false);
    } finally {
        registerBtn.disabled = false;
        btnText.classList.remove("d-none");
        btnLoading.classList.add("d-none");
    }
});

// Show alert message
function showMessage(msg, success) {
    messageBox.classList.remove("d-none", "alert-success", "alert-danger");
    messageBox.classList.add(success ? "alert-success" : "alert-danger");
    messageBox.innerHTML = msg;
    messageBox.scrollIntoView({ behavior: "smooth" });
}
