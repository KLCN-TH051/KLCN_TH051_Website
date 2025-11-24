import BaseApi from "../core/BaseApi.js";

const RoleApi = {
    // 1. Lấy tất cả role + permissions
    getAll: () => {
        return BaseApi.get("roles");
    },

    // 2. Cập nhật toàn bộ quyền cho role
    updatePermissions: (roleName, permissions) => {
        return BaseApi.put(`roles/${roleName}/permissions`, permissions);
    },

    // 3. Xóa 1 quyền riêng lẻ của role
    removePermission: (roleName, permissionValue) => {
        return BaseApi.delete(`roles/${roleName}/permissions/${permissionValue}`);
    },

    // 4. Lấy tất cả giá trị permission hiện có (claim values)
    getClaimValues: () => {
        return BaseApi.get("roles/claim-values");
    }
};

export default RoleApi;
