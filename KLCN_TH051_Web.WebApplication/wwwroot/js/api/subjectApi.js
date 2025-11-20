// wwwroot/js/api/subjectApi.js
window.SubjectApi = {
    // Lấy danh sách tất cả môn học
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

    // Tạo môn học mới
    create: function (data) {
        return BaseApi.post("/Subjects", data)
            .then(res => {
                // giả sử BE trả về { success: true/false, message, data }
                if (res.success === false) {
                    throw { message: res.message };
                }
                return res.data; // trả về object môn học
            });
    },


    // Cập nhật môn học theo id
    update: function (id, data) {
        return BaseApi.put(`/Subjects/${id}`, data)
            .then(res => {
                if (res.success === false) {   // kiểm tra BE trả lỗi
                    throw { message: res.message };
                }
                return res.data;               // trả về môn học đã cập nhật
            });
    },


    // Xóa môn học theo id
    delete: function (id) {
        return BaseApi.delete(`/Subjects/${id}`)
            .then(res => res)
            .catch(err => {
                console.error(`Lỗi khi xóa môn học ID ${id}:`, err);
                return null;
            });
    }
};
