// modules/subject/subject.insert.js
import { loadSubjects } from "./subject.list.js";
import SubjectApi from "../../api/subjectApi.js";
import Toast from "../../components/Toast.js"; // <-- import Toast

export function insertSubject() {
    const name = document.getElementById("subjectNameInput").value.trim();
    const desc = document.getElementById("subjectDescInput").value.trim();

    if (!name) {
        Toast.show("Tên môn học không được để trống!", "danger");
        return Promise.reject("Tên môn học trống"); // trả về Promise để tránh lỗi .then()
    }

    const data = { name, description: desc };

    return SubjectApi.create(data)
        .then(res => {
            Toast.show("Thêm môn học thành công!", "success");
            loadSubjects();
            return res; // trả về kết quả để then() ở init.js nhận
        })
        .catch(err => {
            Toast.show(err?.message || "Lỗi khi thêm môn học", "danger");
            return Promise.reject(err); // trả về rejected Promise
        });
}
