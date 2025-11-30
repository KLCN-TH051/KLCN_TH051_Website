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

    // ---- Load subject ----
    try {
        const subjects = await SubjectApi.getAll();
        subjectList.innerHTML = "";

        if (!subjects.length) {
            subjectList.innerHTML = `<li class="list-group-item text-white">Chưa có môn học nào</li>`;
            return;
        }

        subjects.forEach(s => {
            const li = document.createElement("li");
            li.className = "list-group-item text-white";
            li.style.backgroundColor = "rgb(25 135 84)"; 
            li.textContent = s.name;
            subjectList.appendChild(li);
        });

    } catch (err) {
        console.error(err);
        subjectList.innerHTML = `<li class="list-group-item text-danger">Lỗi khi load môn học</li>`;
    }
});
