import QuizApi from "/js/api/quizApi.js";
import QuestionApi from "/js/api/questionApi.js";
import AnswerApi from "/js/api/answerApi.js";
import Toast from "/js/components/Toast.js";

// ============================
//  GLOBAL STATE
// ============================
window.currentQuizEdit = null;
window.quizModalInstance = null;
window.isQuizModalOpening = false; // ⭐ lock thao tác mở modal nhanh

const modalEl = document.getElementById("editQuizModal");

// Khi modal đóng hoàn toàn → reset state
if (modalEl) {
    modalEl.addEventListener("hidden.bs.modal", () => {
        window.currentQuizEdit = null;
        window.quizModalInstance = null;
        window.isQuizModalOpening = false;

        document.getElementById("quizDescription").value = "";
        document.getElementById("quizQuestionList").innerHTML = "";
        document.getElementById("quizTitle").value = "";
    });
}

// ============================================================
// ĐỔI TÊN QUIZ
// ============================================================
window.renameQuizTitle = async () => {
    if (!window.currentQuizEdit) return;

    const title = document.getElementById("quizTitle")?.value.trim();
    if (!title) {
        Toast.show("Vui lòng nhập tên quiz!", "warning");
        return;
    }

    try {
        await QuizApi.updateQuiz(window.currentQuizEdit.quizId, { title });
        Toast.show("Đã đổi tên Quiz!", "success");
    } catch (err) {
        console.error(err);
        Toast.show("Đổi tên Quiz thất bại!", "danger");
    }
};

// ============================================================
// MỞ MODAL QUIZ (Fix thao tác nhanh)
// ============================================================
window.openQuizModal = async (chapterId, lessonId, title, quiz = null, isNew = false) => {
    if (window.isQuizModalOpening) return; // đang mở modal, ignore
    window.isQuizModalOpening = true;

    try {
        // Reset modal instance nếu còn
        if (window.quizModalInstance) {
            try { window.quizModalInstance.hide(); } catch { }
            window.quizModalInstance = null;
        }

        // State Quiz
        window.currentQuizEdit = {
            chapterId: parseInt(chapterId),
            lessonId: parseInt(lessonId),
            quizId: quiz?.id ?? null,
            isNew,
            questions: []
        };

        window.quizModalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);

        document.getElementById("quizTitle").value = title || quiz?.title || "";
        document.getElementById("quizDescription").value = quiz?.description || "";

        if (isNew) {
            const newQuiz = await QuizApi.createQuiz({
                lessonId,
                title,
                description: document.getElementById("quizDescription").value.trim(),
                type: 3
            });
            window.currentQuizEdit.quizId = newQuiz.id;
            window.currentQuizEdit.isNew = false;
            modalEl.querySelector(".modal-title").textContent = "Chỉnh sửa Quiz";
            Toast.show("Tạo quiz thành công! Bạn có thể chỉnh sửa ngay.", "success");
            await loadQuestionsForQuiz(newQuiz.id);
        } else if (quiz?.id) {
            await loadQuestionsForQuiz(quiz.id);
            modalEl.querySelector(".modal-title").textContent = "Chỉnh sửa Quiz";
        }

        // Xóa backdrop cũ tránh double
        document.querySelectorAll(".modal-backdrop").forEach(b => b.remove());
        window.quizModalInstance.show();

    } catch (err) {
        console.error(err);
        Toast.show("Không thể mở quiz!", "danger");
    } finally {
        window.isQuizModalOpening = false;
    }
};

// ============================================================
// LOAD CÂU HỎI
// ============================================================
async function loadQuestionsForQuiz(quizId) {
    try {
        const list = await QuestionApi.getQuestionsByQuiz(quizId);
        window.currentQuizEdit.questions = await Promise.all(
            list.map(async q => {
                const answers = await AnswerApi.getAnswersByQuestion(q.id);
                return {
                    id: q.id,
                    content: q.questionText,
                    points: q.points,
                    orderNumber: q.orderNumber,
                    answers: answers.map(a => ({
                        id: a.id,
                        content: a.answerText,
                        isCorrect: a.isCorrect
                    }))
                };
            })
        );
        renderQuestions();
    } catch (err) {
        console.error(err);
        Toast.show("Không tải được danh sách câu hỏi!", "danger");
    }
}

// ============================================================
// RENDER DANH SÁCH CÂU HỎI
// ============================================================
function renderQuestions() {
    const container = document.getElementById("quizQuestionList");
    if (!window.currentQuizEdit) return;

    container.innerHTML = window.currentQuizEdit.questions.map((q, index) => `
        <div class="card mb-3 quiz-question-item" data-index="${index}">
            <div class="card-body">
                <div class="d-flex justify-content-between mb-2">
                    <strong>Câu hỏi ${index + 1}</strong>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteQuestion(${index})">Xóa</button>
                </div>
                <textarea class="form-control mb-3" oninput="updateQuestionContent(${index}, this.value)" placeholder="Nhập nội dung câu hỏi...">${q.content}</textarea>
                <div class="answer-list">
                    ${q.answers.map((a, aIndex) => `
                        <div class="input-group mb-2">
                            <span class="input-group-text">
                                <input type="checkbox" ${a.isCorrect ? "checked" : ""} onchange="toggleAnswerCorrect(${index}, ${aIndex}, this.checked)">
                            </span>
                            <input class="form-control" value="${a.content}" oninput="updateAnswerContent(${index}, ${aIndex}, this.value)">
                            <button class="btn btn-outline-danger" onclick="deleteAnswer(${index}, ${aIndex})">X</button>
                        </div>
                    `).join("")}
                </div>
                <button class="btn btn-sm btn-outline-primary mt-2" onclick="addAnswer(${index})">+ Thêm đáp án</button>
            </div>
        </div>
    `).join("");
}

// ============================================================
// THÊM CÂU HỎI
// ============================================================
window.addQuestion = async () => {
    const quizId = window.currentQuizEdit.quizId;
    if (!quizId) return Toast.show("Bạn cần lưu Quiz trước!", "warning");

    try {
        const newQ = await QuestionApi.createQuestion({ quizId, questionText: "Câu hỏi mới", points: 0 });
        window.currentQuizEdit.questions.push({
            id: newQ.id,
            content: newQ.questionText,
            points: newQ.points,
            orderNumber: newQ.orderNumber,
            answers: []
        });
        renderQuestions();
        Toast.show("Thêm câu hỏi thành công!", "success");
    } catch (err) {
        console.error(err);
        Toast.show("Không thể thêm câu hỏi!", "danger");
    }
};

// ============================================================
// UPDATE NỘI DUNG
// ============================================================
window.updateQuestionContent = (index, value) => window.currentQuizEdit.questions[index].content = value;
window.updateAnswerContent = (qIndex, aIndex, value) => window.currentQuizEdit.questions[qIndex].answers[aIndex].content = value;
window.toggleAnswerCorrect = (qIndex, aIndex, checked) => window.currentQuizEdit.questions[qIndex].answers[aIndex].isCorrect = checked;

// ============================================================
// XÓA CÂU HỎI
// ============================================================
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

// ============================================================
// LƯU QUIZ
// ============================================================
window.saveQuiz = async () => {
    if (!window.currentQuizEdit) return;

    const description = document.getElementById("quizDescription").value.trim();
    const title = document.getElementById("quizTitle").value.trim();
    const payload = { title, description, type: 3 };

    try {
        if (window.currentQuizEdit.isNew) {
            await QuizApi.createQuiz({ ...payload, lessonId: window.currentQuizEdit.lessonId });
            Toast.show("Tạo quiz thành công!", "success");
        } else {
            await QuizApi.updateQuiz(window.currentQuizEdit.quizId, payload);

            for (const q of window.currentQuizEdit.questions)
                await QuestionApi.updateQuestion(q.id, { questionText: q.content, points: q.points });

            for (const q of window.currentQuizEdit.questions)
                for (const a of q.answers)
                    await AnswerApi.updateAnswer(a.id, { answerText: a.content, isCorrect: a.isCorrect });

            Toast.show("Cập nhật quiz thành công!", "success");
        }

        if (window.quizModalInstance) window.quizModalInstance.hide();
    } catch (err) {
        console.error(err);
        Toast.show("Lưu quiz thất bại!", "danger");
    }
};

// ============================================================
// MỞ QUIZ TỪ BÀI HỌC
// ============================================================
window.editQuiz = async (lessonId) => {
    try {
        const quizzes = await QuizApi.getQuizzesByLesson(lessonId);
        const quiz = quizzes[0];
        if (!quiz) return Toast.show("Quiz chưa được tạo!", "info");
        window.openQuizModal(quiz.chapterId, lessonId, quiz.title, quiz, false);
    } catch (err) {
        console.error(err);
        Toast.show("Không tải được quiz!", "danger");
    }
};

// ============================================================
// ĐÁP ÁN
// ============================================================
window.addAnswer = async (qIndex) => {
    const question = window.currentQuizEdit.questions[qIndex];
    try {
        const newA = await AnswerApi.createAnswer({ questionId: question.id, answerText: "Đáp án mới", isCorrect: false });
        question.answers.push({ id: newA.id, content: newA.answerText, isCorrect: newA.isCorrect, orderNumber: newA.orderNumber });
        renderQuestions();
        Toast.show("Thêm đáp án thành công!", "success");
    } catch (err) {
        console.error(err);
        Toast.show("Không thể thêm đáp án!", "danger");
    }
};

window.deleteAnswer = async (qIndex, aIndex) => {
    const a = window.currentQuizEdit.questions[qIndex].answers[aIndex];
    try {
        await AnswerApi.deleteAnswer(a.id);
        window.currentQuizEdit.questions[qIndex].answers.splice(aIndex, 1);
        renderQuestions();
        Toast.show("Xóa đáp án thành công!", "success");
    } catch (err) {
        console.error(err);
        Toast.show("Xóa đáp án thất bại!", "danger");
    }
};
