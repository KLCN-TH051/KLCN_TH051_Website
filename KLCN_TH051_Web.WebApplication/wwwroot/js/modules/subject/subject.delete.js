// modules/subject/subject.delete.js
import { loadSubjects } from "./subject.list.js";
import SubjectApi from "../../api/subjectApi.js";
import Toast from "../../components/Toast.js"; // <-- import Toast

export function deleteSubject(id) {
    if (!id) return Promise.reject("Chưa chọn ID xóa");

    return SubjectApi.delete(id)
        .then(() => {
            loadSubjects();
            Toast.show("Xóa môn học thành công!", "success");
        })
        .catch(err => {
            Toast.show(err?.message || "Lỗi khi xóa môn học", "danger");
            return Promise.reject(err);
        });
}
