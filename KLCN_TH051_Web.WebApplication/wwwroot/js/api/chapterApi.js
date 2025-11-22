import BaseApi from "../core/BaseApi.js";

const ChapterApi = {
    // 1. Lấy danh sách chapter theo courseId
    getByCourse(courseId) {
        return BaseApi.get(`courses/${courseId}/chapters`);
    },

    // 2. Lấy chi tiết chapter theo chapterId
    getById(courseId, id) {
        return BaseApi.get(`courses/${courseId}/chapters/${id}`);
    },

    // 3. Tạo chapter mới (POST /api/courses/{courseId}/chapters)
    create(courseId, data) {
        return BaseApi.post(`courses/${courseId}/chapters`, data);
    },

    // 4. Cập nhật chapter
    update(courseId, id, data) {
        return BaseApi.put(`courses/${courseId}/chapters/${id}`, data);
    },

    // 5. Xóa chapter
    delete(id) {
        return BaseApi.delete(`chapters/${id}`);
    },

    // 6. Reorder chapter (POST /api/courses/{courseId}/chapters/reorder)
    reorder(courseId, chapterIdsInNewOrder) {
        return BaseApi.post(`courses/${courseId}/chapters/reorder`, chapterIdsInNewOrder);
    }
};

export default ChapterApi;
