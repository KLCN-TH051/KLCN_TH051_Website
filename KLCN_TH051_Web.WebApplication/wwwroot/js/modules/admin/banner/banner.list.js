import BannerApi from "../../../api/bannerApi.js";
import UploadApi from "../../../api/UploadApi.js";
import Toast from "../../../components/Toast.js";

document.addEventListener("DOMContentLoaded", () => {

    const bannerList = document.getElementById("bannerList");
    const btnCreate = document.getElementById("btnCreate");

    const bannerModal = new bootstrap.Modal(document.getElementById("bannerModal"));
    const bannerIdInput = document.getElementById("bannerId");
    const bannerTitleInput = document.getElementById("bannerTitle");
    const bannerLinkInput = document.getElementById("bannerLink");
    const bannerImageInput = document.getElementById("bannerImage");
    const bannerPreview = document.getElementById("bannerPreview");
    const bannerSaveBtn = document.getElementById("bannerSaveBtn");

    // ================== Load banners ==================
    async function loadBanners() {
        if (!bannerList) return;

        bannerList.innerHTML = `<div class="text-muted">Đang tải...</div>`;

        try {
            const banners = await BannerApi.getAll();

            if (!Array.isArray(banners) || banners.length === 0) {
                bannerList.innerHTML = `<div class="text-muted">Chưa có banner nào</div>`;
                return;
            }

            bannerList.innerHTML = "";

            banners.forEach(b => {
                const card = document.createElement("div");
                card.className = "card mb-2";

                card.innerHTML = `
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h6 class="mb-0 fw-bold">${b.title}</h6>
                        <div>
                            <button class="btn btn-sm btn-outline-primary me-1 editBtn" data-id="${b.id}">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-danger me-1 deleteBtn" data-id="${b.id}">
                                <i class="bi bi-trash"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-secondary" data-bs-toggle="collapse"
                                    data-bs-target="#bannerContent${b.id}">
                                <i class="bi bi-chevron-down"></i>
                            </button>
                        </div>
                    </div>

                    <div id="bannerContent${b.id}" class="collapse">
                        <div class="card-body">
                            ${b.imageUrl ? `<img src="${b.imageUrl}" class="img-fluid rounded mb-2">` : ""}
                            <p><strong>Link:</strong> ${b.linkUrl || "Không có"}</p>
                            <p><strong>Thứ tự:</strong> ${b.order}</p>
                        </div>
                    </div>
                `;

                bannerList.appendChild(card);
            });

            attachEvents();
        } catch (err) {
            console.error(err);
            bannerList.innerHTML = `<div class="text-danger">Lỗi khi load dữ liệu</div>`;
        }
    }

    // ================== Attach Events ==================
    function attachEvents() {
        document.querySelectorAll(".editBtn").forEach(btn => {
            btn.addEventListener("click", async () => {
                const id = btn.dataset.id;
                const banners = await BannerApi.getAll();
                const banner = banners.find(b => b.id == id);

                if (!banner) return Toast.show("Không tìm thấy banner");

                bannerIdInput.value = banner.id;
                bannerTitleInput.value = banner.title;
                bannerLinkInput.value = banner.linkUrl || "";

                if (banner.imageUrl) {
                    bannerPreview.src = banner.imageUrl;
                    bannerPreview.style.display = "block";
                } else {
                    bannerPreview.style.display = "none";
                }

                document.getElementById("bannerModalTitle").innerText = "Sửa Banner";
                bannerModal.show();
            });
        });

        document.querySelectorAll(".deleteBtn").forEach(btn => {
            btn.addEventListener("click", async () => {
                const id = btn.dataset.id;

                if (!confirm("Bạn chắc chắn muốn xóa banner này?")) return;

                try {
                    await BannerApi.delete(id);
                    Toast.show("Xóa banner thành công", "success");
                    loadBanners();
                } catch (err) {
                    console.error(err);
                    Toast.show("Xóa thất bại");
                }
            });
        });
    }

    // ================== Create ==================
    btnCreate.addEventListener("click", () => {
        bannerIdInput.value = "";
        bannerTitleInput.value = "";
        bannerLinkInput.value = "";
        bannerImageInput.value = "";
        bannerPreview.style.display = "none";

        document.getElementById("bannerModalTitle").innerText = "Tạo Banner";
        bannerModal.show();
    });

    // ================== Preview ==================
    bannerImageInput.addEventListener("change", e => {
        const file = e.target.files[0];
        if (file) {
            bannerPreview.src = URL.createObjectURL(file);
            bannerPreview.style.display = "block";
        }
    });

    // ================== Save ==================
    bannerSaveBtn.addEventListener("click", async () => {
        const id = bannerIdInput.value;
        const title = bannerTitleInput.value.trim();
        const linkUrl = bannerLinkInput.value.trim();

        if (!title) return Toast.show("Tiêu đề không được để trống");

        let imageUrl = bannerPreview.src;

        try {
            // upload ảnh mới
            if (bannerImageInput.files.length > 0) {
                const file = bannerImageInput.files[0];

                if (id) {
                    // update
                    const banners = await BannerApi.getAll();
                    const old = banners.find(b => b.id == id);

                    const oldName = old?.imageUrl?.split("/").pop();

                    const uploadRes = await UploadApi.updateFile(file, "banner", oldName);
                    imageUrl = uploadRes.fileUrl;
                } else {
                    const uploadRes = await UploadApi.uploadFile(file, "banner");
                    imageUrl = uploadRes.fileUrl;
                }
            }

            const payload = { title, imageUrl, linkUrl };

            if (id) {
                await BannerApi.update(id, payload);
                Toast.show("Cập nhật banner thành công", "success");
            } else {
                await BannerApi.create(payload);
                Toast.show("Tạo banner thành công", "success");
            }

            bannerModal.hide();
            loadBanners();

        } catch (err) {
            console.error(err);
            Toast.show("Lưu banner thất bại");
        }
    });

    loadBanners();
});
