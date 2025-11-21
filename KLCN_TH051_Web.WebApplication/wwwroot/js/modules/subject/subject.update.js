// modules/subject/subject.update.js
import { loadSubjects } from "./subject.list.js";
import SubjectApi from "../../api/subjectApi.js";
import Toast from "../../components/Toast.js"; // <-- import Toast

export function updateSubject(editId) {
    const name = document.getElementById("subjectNameInput").value.trim();
    const desc = document.getElementById("subjectDescInput").value.trim();

    if (!name) {
        Toast.show("Tên môn học không được để trống!", "danger");
        return Promise.reject("Tên môn học trống");
    }

    const data = { name, description: desc };

    return SubjectApi.update(editId, data)
        .then(res => {
            Toast.show("Cập nhật môn học thành công!", "success");
            loadSubjects();
            return res;
        })
        .catch(err => {
            Toast.show(err?.message || "Lỗi khi cập nhật môn học", "danger");
            return Promise.reject(err);
        });
}
