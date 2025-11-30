import BaseApi from "../core/BaseApi.js";

const LessonApi = {
    // 1. Tạo bài học mới
    createLesson: (chapterId, data) => {
        return BaseApi.post(`chapters/${chapterId}/lessons`, data);
    },

    // 2. Cập nhật nội dung bài học
    updateLessonContent: (chapterId, lessonId, data) => {
        return BaseApi.put(`chapters/${chapterId}/lessons/${lessonId}/content`, data);
    },

    // 3. Cập nhật video bài học
    updateLessonVideo: (chapterId, lessonId, data) => {
        return BaseApi.put(`chapters/${chapterId}/lessons/${lessonId}/video`, data);
    },

    // 4. cập nhật quiz
    updateLessonQuiz(chapterId, lessonId, data) {
        return api.put(`/chapters/${chapterId}/lessons/${lessonId}/quiz`, data);
    },

    // 5. Xóa bài học
    deleteLesson: (chapterId, lessonId) => {
        return BaseApi.delete(`chapters/${chapterId}/lessons/${lessonId}`);
    },

    // 6. Lấy chi tiết bài học theo Id
    getLessonById: (chapterId, lessonId) => {
        return BaseApi.get(`chapters/${chapterId}/lessons/${lessonId}`);
    },

    // 7. Lấy danh sách bài học theo chapter
    getLessonsByChapter: (chapterId) => {
        return BaseApi.get(`chapters/${chapterId}/lessons`);
    },

    //8. 
    getLessonTypes: () => {
        return BaseApi.get("lesson-types");  // gọi /api/lesson-types
    }
};

export default LessonApi;