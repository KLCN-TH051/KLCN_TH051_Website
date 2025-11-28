import SubjectApi from "../../api/subjectApi.js";

export async function initFilter({ allCourses, pagination }) {

    await loadSubjects();
    initEvents(allCourses, pagination);
}

// ----------------------------------------------
// LOAD SUBJECT
// ----------------------------------------------
async function loadSubjects() {
    const container = document.querySelector("#subject-filters");
    container.innerHTML = `<div class="text-muted small">Đang tải...</div>`;

    let subjects = [];

    try {
        subjects = await SubjectApi.getAll();
    } catch (e) {
        console.error("Lỗi loadSubjects:", e);
        container.innerHTML = `<div class="text-danger small">Không thể tải môn học</div>`;
        return;
    }

    if (!subjects || subjects.length === 0) {
        container.innerHTML = `<div class="text-muted small">Không có môn học</div>`;
        return;
    }

    container.innerHTML = subjects.map(sub => `
        <div class="form-check">
            <input class="form-check-input subject-checkbox"
                   type="checkbox"
                   value="${sub.id}"
                   id="subject_${sub.id}">
            <label class="form-check-label" for="subject_${sub.id}">${sub.name}</label>
        </div>
    `).join("");
}

// ----------------------------------------------
// FILTER LOGIC
// ----------------------------------------------
function initEvents(allCourses, pagination) {

    const applyFilters = () => {
        let filtered = [...allCourses];

        // Search
        const keyword = document.querySelector("#txtSearch").value.trim().toLowerCase();
        if (keyword) {
            filtered = filtered.filter(c =>
                c.name?.toLowerCase().includes(keyword) ||
                c.description?.toLowerCase().includes(keyword)
            );
        }

        // Subject filter
        const subjectIds = [...document.querySelectorAll(".subject-checkbox:checked")]
            .map(cb => Number(cb.value));

        if (subjectIds.length > 0) {
            filtered = filtered.filter(c => subjectIds.includes(c.subjectId));
        }

        // Sort (CHỈ GIỮ LẠI GIÁ)
        const sort = document.querySelector("#sortSelect").value;

        if (sort === "priceAsc") {
            filtered.sort((a, b) => (a.price ?? 0) - (b.price ?? 0));
        }
        else if (sort === "priceDesc") {
            filtered.sort((a, b) => (b.price ?? 0) - (a.price ?? 0));
        }

        // Update pagination
        pagination.updateData(filtered);
    };

    // Event: click SEARCH
    document.querySelector("#btnSearch").addEventListener("click", applyFilters);

    // Enter => apply
    document.querySelector("#txtSearch").addEventListener("keyup", e => {
        if (e.key === "Enter") applyFilters();
    });
}
