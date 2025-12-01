import BannerApi from "../../../api/bannerApi.js";
import SubjectApi from "../../../api/subjectApi.js";

document.addEventListener("DOMContentLoaded", async () => {
    const bannerList = document.getElementById("bannerList");
    const bannerIndicators = document.getElementById("bannerIndicators");
    const subjectList = document.getElementById("subjectList");

    // ---- Load banner ----
    try {
        const banners = await BannerApi.getAll();
        if (!banners.length) return;

        bannerList.innerHTML = "";
        bannerIndicators.innerHTML = "";

        banners.forEach((b, index) => {
            // Carousel item
            const div = document.createElement("div");
            div.className = "carousel-item" + (index === 0 ? " active" : "");
            div.innerHTML = `
                <img src="${b.imageUrl}" alt="${b.title}">
                <div class="carousel-caption d-none d-md-block bg-dark bg-opacity-50 rounded p-2">
                    <h5>${b.title}</h5>
                    ${b.linkUrl ? `<a href="${b.linkUrl}" class="btn btn-primary btn-sm">Xem ngay</a>` : ""}
                </div>
            `;
            bannerList.appendChild(div);

            // Indicator
            const li = document.createElement("li");
            li.setAttribute("data-bs-target", "#bannerCarousel");
            li.setAttribute("data-bs-slide-to", index);
            if (index === 0) li.className = "active";
            bannerIndicators.appendChild(li);
        });

    } catch (err) {
        console.error(err);
        bannerList.innerHTML = `<div class="text-danger">Lỗi khi load banner</div>`;
    }

    // --- Load subject ---
    try {
        const subjects = await SubjectApi.getAll();
        subjectList.innerHTML = "";

        if (!subjects.length) {
            // Đảm bảo vẫn giữ style cho trường hợp không có môn học
            const li = document.createElement("li");
            li.className = "list-group-item text-white";
            li.style.backgroundColor = "rgb(25 135 84)";
            li.textContent = "Chưa có môn học nào";
            subjectList.appendChild(li);
            return;
        }

        subjects.forEach(s => {
            const a = document.createElement("a");
            // Thêm đường dẫn đến trang chi tiết môn học. Giả sử object s có thuộc tính 'id'.
            a.href = `subject${s.id}.html`; // Hoặc đường dẫn phù hợp với cấu trúc project của bạn
            a.className = "list-group-item list-group-item-action text-white py-2 px-3"; // Loại bỏ padding mặc định lớn
            a.style.backgroundColor = "rgb(25 135 84)";
            a.textContent = s.name;
            subjectList.appendChild(a);
        });

    } catch (err) {
        console.error(err);
        subjectList.innerHTML = `<li class="list-group-item text-danger">Lỗi khi load môn học</li>`;
    }
});