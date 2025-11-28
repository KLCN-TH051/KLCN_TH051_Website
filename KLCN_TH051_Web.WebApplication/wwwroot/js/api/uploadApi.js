import BaseApi from "../core/BaseApi.js";

const UploadApi = {
    /**
     * Upload file (image/video)
     * @param {File} file - file từ input
     * @param {string} type - "course", "content", "avatar", "video"
     * @returns {Promise<{fileName: string, fileUrl: string}>}
     */
    uploadFile(file, type) {
        if (!file) return Promise.reject("Chưa chọn file");

        const formData = new FormData();
        formData.append("file", file);

        // map type thành endpoint đúng API
        let endpoint = "";
        switch (type.toLowerCase()) {
            case "course":
                endpoint = "CourseImage";
                break;
            case "content":
                endpoint = "ContentImage";
                break;
            case "avatar":
                endpoint = "Avatar";
                break;
            case "video":
                endpoint = "LessonVideo";
                break;
            default:
                return Promise.reject("Type không hợp lệ: course | content | avatar | video");
        }

        // Chú ý: BaseApi.post sẽ tự thêm /api/Upload/ nếu BaseApi config sẵn baseURL
        return BaseApi.post(`Upload/${endpoint}`, formData, { isFormData: true });
    },

    /**
     * Lấy URL file đã upload
     * @param {string} type - loại file
     * @param {string} fileName - tên file trả về từ API
     * @returns {string} url trực tiếp
     */
    getFileUrl(type, fileName) {
        if (!fileName) return "";

        // Map type thành folder tương ứng trên server
        let folder = "";
        switch (type.toLowerCase()) {
            case "course":
                folder = "images/courses";
                break;
            case "content":
                folder = "images/contents";
                break;
            case "avatar":
                folder = "images/avatars";
                break;
            case "video":
                folder = "videos/lessons";
                break;
            default:
                return "";
        }

        return BaseApi.getFileUrl(`${folder}/${fileName}`);
    }
};

export default UploadApi;
