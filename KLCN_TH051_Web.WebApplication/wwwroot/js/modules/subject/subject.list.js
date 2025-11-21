// modules/subject/subject.list.js

import SubjectApi from "../../api/subjectApi.js"; // đường dẫn tương đối tới file subjectApi.js
export function loadSubjects() {
    SubjectApi.getAll().then(subjects => {
        const tableBody = document.getElementById("subjectTableBody");
        tableBody.innerHTML = "";
        subjects.forEach(sub => {
            const row = `<tr>
                <td>${sub.id}</td>
                <td>${sub.name}</td>
                <td>${sub.description || ""}</td>
                <td>${new Date(sub.createdDate).toLocaleString()}</td>
                <td class="d-flex">
                    <button class="btn btn-sm btn-primary me-1" data-edit='${JSON.stringify(sub)}'>Sửa</button>
                    <button class="btn btn-sm btn-danger" data-delete='${sub.id}'>Xóa</button>
                </td>
            </tr>`;
            tableBody.innerHTML += row;
        });
    });
}
