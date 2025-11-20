// wwwroot/js/api/TeacherAssignments.js

window.TeacherAssignmentsApi = {

    // Lấy tất cả phân công
    getAll: function () {
        return BaseApi.get("TeacherAssignments");
    },

    // Tạo phân công mới
    create: function (data) {
        // data = { teacherId: ..., subjectId: ... }
        return BaseApi.post("TeacherAssignments", data);
    }

};
