// wwwroot/js/core/BaseApi.js
const BaseApi = {
    getToken() {
        return localStorage.getItem("authToken") || "";
    },

    setToken(token) {
        localStorage.setItem("authToken", token);
    },

    getFullUrl(path) {
        let url = window.API_URL || "";
        if (!url.endsWith("/")) url += "/";
        if (path.startsWith("/")) path = path.substring(1);
        return url + "api/" + path;
    },

    handleResponse(res) {
        if (!res.ok) {
            return res.text().then(text => {
                throw new Error(text || "Lỗi khi gọi API");
            });
        }
        return res.status === 204 ? null : res.json();
    },

    get(path) {
        return fetch(this.getFullUrl(path), {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            }
        }).then(this.handleResponse);
    },

    post(path, data) {
        return fetch(this.getFullUrl(path), {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    put(path, data) {
        return fetch(this.getFullUrl(path), {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    patch(path, data) {
        return fetch(this.getFullUrl(path), {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            },
            body: JSON.stringify(data)
        }).then(this.handleResponse);
    },

    delete(path) {
        return fetch(this.getFullUrl(path), {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.getToken()
            }
        }).then(this.handleResponse);
    }
};

// Xuất module để import
export default BaseApi;
