// wwwroot/js/modules/teacherAssignments/teacherAssignments.dropdown.js
import AccountApi from "../../api/AccountApi.js";
import SubjectApi from "../../api/SubjectApi.js";

export function loadTeacherAndSubjectDropdowns() {
    const teacherSelect = document.querySelector("#teacherSelect");
    const subjectSelect = document.querySelector("#subjectSelect");

    const teachersPromise = AccountApi.getTeachers().then(teachers => {
        teacherSelect.innerHTML = "";
        teachers.forEach(t => {
            const option = document.createElement("option");
            option.value = t.id;
            option.textContent = t.fullName;
            teacherSelect.appendChild(option);
        });
    });

    const subjectsPromise = SubjectApi.getAll().then(subjects => {
        subjectSelect.innerHTML = "";
        subjects.forEach(s => {
            const option = document.createElement("option");
            option.value = s.id;
            option.textContent = s.name;
            subjectSelect.appendChild(option);
        });
    });

    // Trả về Promise tổng hợp để init modal có thể dùng .then()
    return Promise.all([teachersPromise, subjectsPromise]);
}