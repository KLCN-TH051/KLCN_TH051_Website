// wwwroot/js/core/BaseApi.js
window.BaseApi = {
    getToken: function () {
        return localStorage.getItem("authToken") || "";
    },

    setToken: function (token) {
        localStorage.setItem("authToken", token);
    },

    getFullUrl: function (path) {
        let url = window.API_URL;
        if (!url.endsWith("/")) url += "/";
        if (path.startsWith("/")) path = path.substring(1);
        return url + "api/" + path;
    },

    handleResponse: function (res) {
        if (!res.ok) {
            return res.text().then(text => {
                throw new Error(text || "Lỗi khi gọi API");
            });
        }
        // Nếu body rỗng (status 204), trả về null
        return res.status === 204 ? null : res.json();
    },

    get: function (path) {
        return fetch(this.getFullUrl(path), {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            }
        }).then(this.handleResponse);
    },

    post: function (path, data) {
        return fetch(this.getFullUrl(path), {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    put: function (path, data) {
        return fetch(this.getFullUrl(path), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    patch: function (path, data) {
        return fetch(this.getFullUrl(path), {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    delete: function (path) {
        return fetch(this.getFullUrl(path), {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            }
        }).then(this.handleResponse);
    }
};
