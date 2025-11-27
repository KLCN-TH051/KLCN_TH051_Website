// wwwroot/js/modules/instructor/quiz/quiz.edit.js

import QuizApi from "/js/api/quizApi.js";
import QuestionApi from "/js/api/questionApi.js";
import Toast from "/js/components/Toast.js";

window.currentQuizEdit = null;

/* ============================================================
   1) MỞ MODAL QUIZ
============================================================ */
window.openQuizModal = async (chapterId, lessonId, title, quiz = null, isNew = false) => {

    window.currentQuizEdit = {
        chapterId: parseInt(chapterId),
        lessonId: parseInt(lessonId),
        quizId: quiz?.id ?? null,
        isNew,
        questions: []
    };

    const modalEl = document.getElementById("editQuizModal");
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    // Mô tả
    document.getElementById("quizDescription").value = quiz?.description || "";

    /* ------------------------------------------------------------
        LẤY DANH SÁCH CÂU HỎI (API riêng)
    ------------------------------------------------------------ */
    if (!isNew && quiz?.id) {
        try {
            const list = await QuestionApi.getQuestionsByQuiz(quiz.id);

            window.currentQuizEdit.questions = list.map((q) => ({
                id: q.id,
                content: q.questionText,
                points: q.points,
                orderNumber: q.orderNumber
            }));

        } catch (err) {
            console.error(err);
            Toast.show("Không tải được danh sách câu hỏi!", "danger");
        }
    }

    renderQuestions();

    modalEl.querySelector(".modal-title").textContent = isNew ? "Tạo Quiz mới" : "Chỉnh sửa Quiz";
    modal.show();

    modalEl.addEventListener("hidden.bs.modal", () => {
        window.currentQuizEdit = null;
        document.getElementById("quizDescription").value = "";
        document.getElementById("quizQuestionList").innerHTML = "";
    }, { once: true });
};

/* ============================================================
   2) RENDER DANH SÁCH CÂU HỎI
============================================================ */
function renderQuestions() {
    const container = document.getElementById("quizQuestionList");

    if (!window.currentQuizEdit) return;

    const questions = window.currentQuizEdit.questions;

    container.innerHTML = questions
        .map((q, index) => `
            <div class="card mb-3 quiz-question-item" data-index="${index}">
                <div class="card-body">

                    <div class="d-flex justify-content-between mb-2">
                        <strong>Câu hỏi ${index + 1}</strong>
                        <button class="btn btn-sm btn-outline-danger" onclick="deleteQuestion(${index})">Xóa</button>
                    </div>

                    <textarea class="form-control question-content"
                        oninput="updateQuestionContent(${index}, this.value)"
                        placeholder="Nhập nội dung câu hỏi...">${q.content}</textarea>

                </div>
            </div>
        `)
        .join("");
}

/* ============================================================
   3) THÊM CÂU HỎI
============================================================ */
window.addQuestion = async () => {
    const quizId = window.currentQuizEdit.quizId;

    if (!quizId) {
        Toast.show("Bạn cần lưu Quiz trước khi thêm câu hỏi!", "warning");
        return;
    }

    try {
        const newQ = await QuestionApi.createQuestion({
            quizId,
            questionText: "Câu hỏi mới",
            points: 0
        });

        window.currentQuizEdit.questions.push({
            id: newQ.id,
            content: newQ.questionText,
            points: newQ.points,
            orderNumber: newQ.orderNumber
        });

        renderQuestions();
        Toast.show("Thêm câu hỏi thành công!", "success");

    } catch (err) {
        console.error(err);
        Toast.show("Không thể thêm câu hỏi!", "danger");
    }
};

/* ============================================================
   4) CẬP NHẬT NỘI DUNG CÂU HỎI
============================================================ */
window.updateQuestionContent = (index, value) => {
    window.currentQuizEdit.questions[index].content = value;
};

/* ============================================================
   5) XÓA CÂU HỎI
============================================================ */
window.deleteQuestion = async (index) => {
    const q = window.currentQuizEdit.questions[index];

    try {
        await QuestionApi.deleteQuestion(q.id);

        window.currentQuizEdit.questions.splice(index, 1);

        renderQuestions();
        Toast.show("Xóa câu hỏi thành công!", "success");

    } catch (err) {
        console.error(err);
        Toast.show("Xóa câu hỏi thất bại!", "danger");
    }
};

/* ============================================================
   6) LƯU QUIZ (CREATE / UPDATE)
============================================================ */
window.saveQuiz = async () => {
    if (!window.currentQuizEdit) return;

    const description = document.getElementById("quizDescription").value.trim();

    // build payload gửi BE
    const payload = {
        title: "",
        description,
        type: 3
    };

    try {
        if (window.currentQuizEdit.isNew) {
            await QuizApi.createQuiz({
                ...payload,
                lessonId: window.currentQuizEdit.lessonId
            });

            Toast.show("Tạo quiz thành công!", "success");
        } else {
            await QuizApi.updateQuiz(window.currentQuizEdit.quizId, payload);

            Toast.show("Cập nhật quiz thành công!", "success");
        }

        bootstrap.Modal.getInstance(document.getElementById("editQuizModal")).hide();

    } catch (err) {
        console.error(err);
        Toast.show("Lưu quiz thất bại!", "danger");
    }
};

/* ============================================================
   7) MỞ EDIT QUIZ TỪ DANH SÁCH BÀI HỌC
============================================================ */
window.editQuiz = async (lessonId) => {
    try {
        const quizzes = await QuizApi.getQuizzesByLesson(lessonId);
        const quiz = quizzes[0];

        if (!quiz) {
            Toast.show("Quiz chưa được tạo!", "info");
            return;
        }

        window.openQuizModal(
            quiz.chapterId,
            lessonId,
            "",
            quiz,
            false
        );

    } catch (err) {
        console.error(err);
        Toast.show("Không tải được quiz!", "danger");
    }
};
