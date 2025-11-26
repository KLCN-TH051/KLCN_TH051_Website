import BaseApi from "../core/BaseApi.js";

const QuizApi = {
    // 1. Tạo quiz mới
    createQuiz: (quizData) => {
        // quizData là object theo CreateQuizRequest
        return BaseApi.post("quiz", quizData);
    },

    // 2. Lấy danh sách quiz theo lesson
    getQuizzesByLesson: (lessonId) => {
        return BaseApi.get(`quiz/lesson/${lessonId}`);
    },

    // 3. Lấy chi tiết quiz
    getQuizById: (quizId) => {
        return BaseApi.get(`quiz/${quizId}`);
    },

    // 4. Cập nhật quiz
    updateQuiz: (quizId, quizData) => {
        // quizData là object theo UpdateQuizRequest
        return BaseApi.put(`quiz/${quizId}`, quizData);
    },

    // 5. Xóa quiz (soft delete)
    deleteQuiz: (quizId) => {
        return BaseApi.delete(`quiz/${quizId}`);
    }
};

export default QuizApi;
