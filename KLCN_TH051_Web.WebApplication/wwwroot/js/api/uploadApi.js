import BaseApi from "../core/BaseApi.js";

const UploadApi = {
    // Upload ảnh khóa học
    uploadCourseImage(file) {
        const formData = new FormData();
        formData.append("file", file);
        return BaseApi.post("Upload/CourseImage", formData, { isFormData: true });
    },

    // Upload ảnh nội dung bài học / content block
    uploadContentImage(file) {
        const formData = new FormData();
        formData.append("file", file);
        return BaseApi.post("Upload/ContentImage", formData, { isFormData: true });
    },

    // Upload avatar người dùng
    uploadAvatar(file) {
        const formData = new FormData();
        formData.append("file", file);
        return BaseApi.post("Upload/Avatar", formData, { isFormData: true });
    },

    // Lấy URL đầy đủ file static
    getFileUrl(path) {
        return BaseApi.getFileUrl(path);
    }
};

export default UploadApi;
