// wwwroot/js/api/contentblockApi.js
import BaseApi from "../core/BaseApi.js";

const ContentBlockApi = {
    // ---------------------------------------------------------
    // Tạo ContentBlock mới
    // POST /api/ContentBlock
    // ---------------------------------------------------------
    create(data) {
        return BaseApi.post("ContentBlock", data);
    },

    // ---------------------------------------------------------
    // Lấy danh sách ContentBlock theo Lesson
    // GET /api/ContentBlock/lesson/{lessonId}
    // ---------------------------------------------------------
    getByLesson(lessonId) {
        return BaseApi.get(`ContentBlock/lesson/${lessonId}`);
    },

    // ---------------------------------------------------------
    // Lấy chi tiết 1 ContentBlock
    // GET /api/ContentBlock/{id}
    // ---------------------------------------------------------
    getById(id) {
        return BaseApi.get(`ContentBlock/${id}`);
    },

    // ---------------------------------------------------------
    // Cập nhật ContentBlock
    // PUT /api/ContentBlock/{id}
    // ---------------------------------------------------------
    update(id, data) {
        return BaseApi.put(`ContentBlock/${id}`, data);
    },

    // ---------------------------------------------------------
    // Xóa ContentBlock (soft delete)
    // DELETE /api/ContentBlock/{id}
    // ---------------------------------------------------------
    delete(id) {
        return BaseApi.delete(`ContentBlock/${id}`);
    },

    // ---------------------------------------------------------
    // Reorder ContentBlocks trong lesson
    // POST /api/ContentBlock/reorder/{lessonId}
    // BODY: [id1, id2, ...]
    // ---------------------------------------------------------
    reorder(lessonId, newOrderList) {
        return BaseApi.post(
            `ContentBlock/reorder/${lessonId}`,
            newOrderList,
            { isFormData: false }
        );
    }
};

export default ContentBlockApi;
