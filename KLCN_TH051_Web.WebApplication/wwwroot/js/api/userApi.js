import BaseApi from "../core/BaseApi.js";

const UserApi = {
    // Lấy tất cả tài khoản
    getAll: () => {
        return BaseApi.get("users");
    },

    // Lấy chi tiết tài khoản theo Id
    getById: (id) => {
        return BaseApi.get(`users/${id}`);
    },

    // Tạo tài khoản mới
    create: (data) => {
        return BaseApi.post("users", data);
    },

    // Cập nhật tài khoản theo Id
    update: (id, data) => {
        return BaseApi.put(`users/${id}`, data);
    },

    // Xóa tài khoản theo Id
    delete: (id) => {
        return BaseApi.delete(`users/${id}`);
    },

    // Lấy danh sách giáo viên
    getTeachers: () => {
        return BaseApi.get("users/teachers");
    }
};

export default UserApi;
