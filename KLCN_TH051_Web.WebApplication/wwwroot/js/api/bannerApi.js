import BaseApi from "../core/BaseApi.js";

const BannerApi = {
    // Lấy danh sách tất cả banner
    getAll() {
        return BaseApi.get("banners");
    },

    // Tạo banner mới
    create(data) {
        return BaseApi.post("banners", data);
    },

    // Cập nhật banner
    update(id, data) {
        return BaseApi.put(`banners/${id}`, data);
    },

    // Xóa banner
    delete(id) {
        return BaseApi.delete(`banners/${id}`);
    },

    // Thay đổi thứ tự banner
    reorder(id, newOrder) {
        return BaseApi.put(`banners/${id}/reorder/${newOrder}`);
    }
};

export default BannerApi;
