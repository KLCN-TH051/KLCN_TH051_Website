import SubjectApi from "../../../api/subjectApi.js";

document.addEventListener("DOMContentLoaded", async () => {
    const subjectList = document.getElementById("subjectList");
    if (!subjectList) return;

    subjectList.innerHTML = `<li class="list-group-item text-muted">Đang tải...</li>`;

    try {
        const subjects = await SubjectApi.getAll(); // gọi API lấy môn học
        subjectList.innerHTML = "";

        if (!subjects || subjects.length === 0) {
            subjectList.innerHTML = `<li class="list-group-item text-muted">Chưa có môn học nào</li>`;
            return;
        }

        subjects.forEach(s => {
            const li = document.createElement("li");
            li.className = "list-group-item";
            li.textContent = s.name; // tên môn học
            subjectList.appendChild(li);
        });

    } catch (err) {
        console.error(err);
        subjectList.innerHTML = `<li class="list-group-item text-danger">Lỗi khi load môn học</li>`;
    }
});
