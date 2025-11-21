// wwwroot/js/api/subjectApi.js
import BaseApi from "../core/BaseApi.js";

const SubjectApi = {
    getAll: function () {
        return BaseApi.get("/Subjects")
            .then(res => {
                if (res.success === false) {
                    console.error("Lỗi khi lấy danh sách môn học:", res.message);
                    return [];
                }
                return res; // trả về mảng subjects
            })
            .catch(err => {
                console.error("Lỗi kết nối API:", err);
                return [];
            });
    },

    create: function (data) {
        return BaseApi.post("/Subjects", data)
            .then(res => {
                if (res.success === false) {
                    throw { message: res.message };
                }
                return res.data;
            });
    },

    update: function (id, data) {
        return BaseApi.put(`/Subjects/${id}`, data)
            .then(res => {
                if (res.success === false) {
                    throw { message: res.message };
                }
                return res.data;
            });
    },

    delete: function (id) {
        return BaseApi.delete(`/Subjects/${id}`)
            .then(res => res)
            .catch(err => {
                console.error(`Lỗi khi xóa môn học ID ${id}:`, err);
                return null;
            });
    }
};

// Xuất module để các file con import
export default SubjectApi;
