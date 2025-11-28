// wwwroot/js/api/questionApi.js
import BaseApi from "../core/BaseApi.js";

const QuestionApi = {
    // 1. Tạo câu hỏi mới
    createQuestion: (questionData) => {
        // questionData là object theo CreateQuestionRequest
        return BaseApi.post("Question", questionData);
    },

    // 2. Lấy danh sách câu hỏi theo quiz
    getQuestionsByQuiz: (quizId) => {
        return BaseApi.get(`Question/quiz/${quizId}`);
    },

    // 3. Lấy chi tiết câu hỏi theo id
    getQuestionById: (id) => {
        return BaseApi.get(`Question/${id}`);
    },

    // 4. Cập nhật câu hỏi
    updateQuestion: (id, questionData) => {
        return BaseApi.put(`Question/${id}`, questionData);
    },

    // 5. Xóa câu hỏi
    deleteQuestion: (id) => {
        return BaseApi.delete(`Question/${id}`);
    },

    // 6. Sắp xếp lại câu hỏi (reorder)
    reorderQuestions: (quizId, orderedQuestionIds) => {
        // orderedQuestionIds: mảng id theo thứ tự mới
        return BaseApi.post(`Question/reorder/${quizId}`, orderedQuestionIds);
    }
};

export default QuestionApi;
