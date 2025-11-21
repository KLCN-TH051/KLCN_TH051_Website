// wwwroot/js/api/courseApi.js
import BaseApi from "../core/BaseApi.js";

const CourseApi = {

    async getTeacherCourses() {
        try {
            return await BaseApi.get("course/teacher");
        } catch (err) {
            console.error("Lỗi getTeacherCourses:", err);
            throw err;
        }
    },

    async createDraft(data) {
        try {
            return await BaseApi.post("course/draft", data);
        } catch (err) {
            console.error("Lỗi createDraft:", err);
            throw err;
        }
    },

    async update(id, data) {
        try {
            return await BaseApi.put(`course/${id}`, data);
        } catch (err) {
            console.error("Lỗi update:", err);
            throw err;
        }
    },

    async submit(id) {
        try {
            return await BaseApi.post(`course/${id}/submit`, {});
        } catch (err) {
            console.error("Lỗi submit course:", err);
            throw err;
        }
    },

    async updateStatus(id, status) {
        try {
            return await BaseApi.post(`course/${id}/status?status=${status}`, {});
        } catch (err) {
            console.error("Lỗi updateStatus:", err);
            throw err;
        }
    },

    async delete(id) {
        try {
            return await BaseApi.delete(`course/${id}`);
        } catch (err) {
            console.error("Lỗi delete course:", err);
            throw err;
        }
    },

    async getById(id) {
        try {
            return await BaseApi.get(`course/${id}`);
        } catch (err) {
            console.error("Lỗi getById:", err);
            throw err;
        }
    },

    async getAll() {
        try {
            return await BaseApi.get("course/all");
        } catch (err) {
            console.error("Lỗi getAll:", err);
            throw err;
        }
    },

    async getApproved() {
        try {
            return await BaseApi.get("course/approved");
        } catch (err) {
            console.error("Lỗi getApproved:", err);
            throw err;
        }
    }
};

export default CourseApi;
