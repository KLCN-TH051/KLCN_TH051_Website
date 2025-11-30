// wwwroot/js/api/AccountApi.js
import BaseApi from "../core/BaseApi.js";

const AccountApi = {

    // Lấy danh sách giáo viên
    getTeachers() {
        return BaseApi.get("Account/teachers")
            .catch(err => {
                console.error("Lỗi khi lấy danh sách giáo viên:", err);
                return []; // trả về mảng rỗng nếu lỗi
            });
    },

    // 🟢 Lấy thông tin profile người dùng đang đăng nhập
    getProfile() {
        return BaseApi.get("Account/profile")
            .then(res => res) // trả về dữ liệu từ API
            .catch(err => {
                console.error("Lỗi khi lấy thông tin profile:", err);
                return null; // trả về null để xử lý ở UI
            });
    }
};

export default AccountApi;
