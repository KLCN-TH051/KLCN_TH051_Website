import BaseApi from "../core/BaseApi.js";

const UploadApi = {
    /**
     * Upload file hình ảnh
     * @param {File} file - file từ input
     * @param {string} type - "course", "content", "avatar", "banner"
     * @returns {Promise<{fileName: string, fileUrl: string}>}
     */
    uploadFile(file, type) {
        if (!file) return Promise.reject("Chưa chọn file");

        const formData = new FormData();
        formData.append("file", file);

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
            case "banner":
                endpoint = "BannerImage";
                break;
            default:
                return Promise.reject("Type không hợp lệ: course | content | avatar | banner");
        }

        return BaseApi.post(`Upload/${endpoint}`, formData, { isFormData: true });
    },

    /**
     * Upload file Excel câu hỏi + đáp án
     * @param {File} file - file Excel từ input
     * @returns {Promise<{fileName: string, fileUrl: string, questionCount: number, answerCount: number}>}
     */
    uploadQuestionsExcel(file) {
        if (!file) return Promise.reject("Chưa chọn file");

        const formData = new FormData();
        formData.append("file", file);

        return BaseApi.post("Upload/QuestionsExcel", formData, { isFormData: true });
    },

    /**
     * Lấy URL file đã upload
     * @param {string} type - loại file
     * @param {string} fileName - tên file trả về từ API
     * @returns {string} url trực tiếp
     */
    getFileUrl(type, fileName) {
        if (!fileName) return "";

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
            case "banner":
                folder = "images/banners";
                break;
            default:
                return "";
        }

        return BaseApi.getFileUrl(`${folder}/${fileName}`);
    },

    /**
 * Update file hình ảnh
 * @param {File} file - file mới
 * @param {string} type - "course", "avatar", "thumbnail", "banner"
 * @param {string} oldFileName - tên file cũ cần update
 * @returns {Promise<{fileName: string, fileUrl: string}>}
 */
updateFile(file, type, oldFileName) {
        if (!file) return Promise.reject("Chưa chọn file");
        if (!oldFileName) return Promise.reject("Chưa có file cũ để cập nhật");

        const formData = new FormData();
        formData.append("file", file);

        let endpoint = "";
        switch (type.toLowerCase()) {
            case "course":
                endpoint = `CourseImage/${oldFileName}`;
                break;
            case "avatar":
                endpoint = `Avatar/${oldFileName}`;
                break;
            case "thumbnail":
                endpoint = `Thumbnail/${oldFileName}`;
                break;
            case "banner":
                endpoint = `BannerImage/${oldFileName}`;
                break;
            default:
                return Promise.reject("Type không hợp lệ: course | avatar | thumbnail | banner");
        }

        return BaseApi.put(`Upload/${endpoint}`, formData, { isFormData: true });
    }

};

export default UploadApi;
