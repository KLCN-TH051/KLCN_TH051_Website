import BaseApi from "../core/BaseApi.js";

const TeacherAssignmentsApi = {

    // Lấy tất cả phân công
    getAll() {
        return BaseApi.get("TeacherAssignments")
            .catch(err => {
                console.error("Lỗi khi lấy danh sách phân công:", err);
                return [];
            });
    },

    // Lấy phân công theo ID
    getById(id) {
        if (!id) {
            console.warn("getById: ID trống!");
            return Promise.resolve(null);
        }

        return BaseApi.get(`TeacherAssignments/${id}`)
            .catch(err => {
                console.error(`Lỗi khi lấy phân công ID ${id}:`, err);
                return null;
            });
    },

    // Lấy danh sách môn học theo giáo viên
    getSubjectsByTeacher(teacherId) {
        return BaseApi.get(`TeacherAssignments/teacher/${teacherId}`)
            .catch(err => {
                console.error(`Lỗi khi lấy môn học của giáo viên ID ${teacherId}:`, err);
                return [];
            });
    },

    // Tạo phân công mới
    create(data) {
        return BaseApi.post("TeacherAssignments", data)
            .catch(err => {
                console.error("Lỗi khi tạo phân công:", err);
                return null;
            });
    },

    // Cập nhật phân công
    update(id, data) {
        return BaseApi.put(`TeacherAssignments/${id}`, data)
            .catch(err => {
                console.error(`Lỗi khi cập nhật phân công ID ${id}:`, err);
                return null;
            });
    },

    // Xóa phân công
    remove(id) {
        return BaseApi.delete(`TeacherAssignments/${id}`)
            .catch(err => {
                console.error(`Lỗi khi xóa phân công ID ${id}:`, err);
                return null;
            });
    }

};

export default TeacherAssignmentsApi;
