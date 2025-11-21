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
    }

};

export default AccountApi;
