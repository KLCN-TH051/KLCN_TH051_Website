// wwwroot/js/api/answerApi.js
import BaseApi from "../core/BaseApi.js";

const AnswerApi = {
    // 1. Tạo đáp án mới
    createAnswer: (answerData) => {
        // answerData: CreateAnswerRequest
        return BaseApi.post("Answer", answerData);
    },

    // 2. Lấy danh sách đáp án theo questionId
    getAnswersByQuestion: (questionId) => {
        return BaseApi.get(`Answer/question/${questionId}`);
    },

    // 3. Lấy chi tiết đáp án theo id
    getAnswerById: (id) => {
        return BaseApi.get(`Answer/${id}`);
    },

    // 4. Cập nhật đáp án
    updateAnswer: (id, answerData) => {
        return BaseApi.put(`Answer/${id}`, answerData);
    },

    // 5. Xóa đáp án
    deleteAnswer: (id) => {
        return BaseApi.delete(`Answer/${id}`);
    }
};

export default AnswerApi;
