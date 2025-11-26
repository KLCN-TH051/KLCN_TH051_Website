import BaseApi from "../core/BaseApi.js";

const QuizApi = {
    async getQuiz(lessonId, data) {
        try {
            return await BaseApi.get(`Quiz/lesson/${lessonId}`, data);
        } catch (err) {
            console.error("Lỗi getQuiz:", err);
            throw err;
        }
    },
    async update(id, data) {
        try {
            return await BaseApi.put(`Quiz/${id}`, data);
        } catch (err) {
            console.error("Lỗi update:", err);
            throw err;
        }
    },
    async delete(id) {
        try {
            return await BaseApi.delete(`Quiz/${id}`);
        } catch (err) {
            console.error("Lỗi delete:", err);
            throw err;
        }
    },
}

export default QuizApi;