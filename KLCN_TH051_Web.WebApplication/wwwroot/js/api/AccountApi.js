// wwwroot/js/api/AccountApi.js

window.AccountApi = {

    // Lấy danh sách giáo viên
    getTeachers: function () {
        return BaseApi.get("Account/teachers");
    }

};
