//import BaseApi from "../core/BaseApi.js";

//const LessonApi = {
//    // ========================
//    // CREATE LESSON
//    // POST /api/chapters/{chapterId}/lessons
//    // ========================
//    createLesson(chapterId, data) {
//        return BaseApi.post(`chapters/${chapterId}/lessons`, data);
//    },

//    // ========================
//    // GET LIST LESSONS
//    // GET /api/chapters/{chapterId}/lessons
//    // ========================
//    getLessons(chapterId) {
//        return BaseApi.get(`chapters/${chapterId}/lessons`);
//    },

//    // ========================
//    // GET LESSON DETAIL
//    // GET /api/chapters/{chapterId}/lessons/{id}
//    // ========================
//    getLessonById(chapterId, lessonId) {
//        return BaseApi.get(`chapters/${chapterId}/lessons/${lessonId}`);
//    },

//    // ========================
//    // UPDATE LESSON
//    // PUT /api/chapters/{chapterId}/lessons/{id}
//    // ========================
//    updateLesson(chapterId, lessonId, data) {
//        return BaseApi.put(`chapters/${chapterId}/lessons/${lessonId}`, data);
//    },

//    // ========================
//    // DELETE LESSON
//    // DELETE /api/chapters/{chapterId}/lessons/{id}
//    // ========================
//    deleteLesson(chapterId, lessonId) {
//        return BaseApi.delete(`chapters/${chapterId}/lessons/${lessonId}`);
//    },

//    // ========================
//    // REORDER LESSONS
//    // POST /api/chapters/{chapterId}/lessons/reorder
//    // BODY: [3,1,2]
//    // ========================
//    reorderLessons(chapterId, lessonIds) {
//        return BaseApi.post(
//            `chapters/${chapterId}/lessons/reorder`,
//            lessonIds
//        );
//    }
//};

//export default LessonApi;
