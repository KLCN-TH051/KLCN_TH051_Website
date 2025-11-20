// wwwroot/js/api/TeacherAssignments.js

window.TeacherAssignmentsApi = {

    // Lấy tất cả phân công
    getAll: function () {
        return BaseApi.get("TeacherAssignments");
    },

    // Lấy phân công theo ID
    getById: function (id) {
        return BaseApi.get(`TeacherAssignments/${id}`);
    },

    // Tạo phân công mới
    create: function (data) {
        return BaseApi.post("TeacherAssignments", data);
    },

    // Cập nhật phân công
    update: function (id, data) {
        return BaseApi.put(`TeacherAssignments/${id}`, data);
    },

    // Xóa phân công
    remove: function (id) {
        return BaseApi.delete(`TeacherAssignments/${id}`);
    },

    // Lấy danh sách môn học theo giáo viên
    getSubjectsByTeacher: function (teacherId) {
        return BaseApi.get(`TeacherAssignments/teacher/${teacherId}`);
    }

};
