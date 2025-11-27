import QuizApi from "/js/api/quizApi.js";
import Toast from "/js/components/Toast.js";

window.currentQuizEdit = null;

/* ============================================================
   MỞ MODAL QUIZ (TẠO MỚI HOẶC SỬA)
============================================================ */
window.openQuizModal = (chapterId, lessonId, title, quiz = null, isNew = false) => {
    window.currentQuizEdit = {
        chapterId: parseInt(chapterId),
        lessonId: parseInt(lessonId),
        quizId: quiz?.id ?? null,
        isNew
    };

    const modalEl = document.getElementById("editQuizModal");
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    // --- Title quiz: ẩn và không dùng → luôn để rỗng ---
    document.getElementById("quizTitle").value = "";

    // --- Description ---
    document.getElementById("quizDescription").value = quiz?.description || "";

    // --- Reset khu vực câu hỏi ---
    const qList = document.getElementById("quizQuestionList");
    qList.innerHTML = "";

    // --- Nếu đang sửa quiz cũ → load câu hỏi ---
    if (quiz?.questions?.length > 0) {
        renderQuestions(quiz.questions);
    }

    // Tiêu đề modal
    modalEl.querySelector(".modal-title").textContent = isNew ? "Tạo Quiz mới" : "Chỉnh sửa Quiz";

    modal.show();

    // Reset khi đóng modal
    modalEl.addEventListener("hidden.bs.modal", () => {
        window.currentQuizEdit = null;
        document.getElementById("quizDescription").value = "";
        document.getElementById("quizQuestionList").innerHTML = "";
    }, { once: true });
};

/* ============================================================
   RENDER DANH SÁCH CÂU HỎI
============================================================ */
function renderQuestions(questions) {
    const container = document.getElementById("quizQuestionList");
    container.innerHTML = "";

    questions.forEach((q, index) => {
        container.insertAdjacentHTML("beforeend", questionTemplate(q, index));
    });
}

function questionTemplate(question, index) {
    const optsHtml = (question.options || [])
        .map((opt, i) => {
            return `
                <div class="input-group mb-2">
                    <span class="input-group-text">${String.fromCharCode(65 + i)}</span>
                    <input class="form-control option-text" value="${opt.text || ""}" data-opt-id="${opt.id}">
                    <span class="input-group-text">
                        <input type="checkbox" class="form-check-input option-correct" ${opt.isCorrect ? "checked" : ""}>
                    </span>
                </div>
            `;
        })
        .join("");

    return `
        <div class="card mb-3 quiz-question-item" data-q-id="${question.id}">
            <div class="card-body">
                <label class="form-label">Câu hỏi ${index + 1}</label>
                <textarea class="form-control mb-2 question-content">${question.content || ""}</textarea>

                <div class="question-options">
                    ${optsHtml}
                </div>

                <button class="btn btn-sm btn-outline-primary" onclick="addOption(this)">
                    + Thêm đáp án
                </button>

                <button class="btn btn-sm btn-outline-danger float-end" onclick="removeQuestion(this)">
                    Xóa câu hỏi
                </button>
            </div>
        </div>
    `;
}

/* ============================================================
   THÊM / XÓA CÂU HỎI
============================================================ */
window.addQuestion = () => {
    const container = document.getElementById("quizQuestionList");

    const blankQ = {
        id: null,
        content: "",
        options: []
    };

    container.insertAdjacentHTML("beforeend", questionTemplate(blankQ, container.children.length));
};

window.removeQuestion = (btn) => {
    btn.closest(".quiz-question-item")?.remove();
};

/* ============================================================
   THÊM ĐÁP ÁN
============================================================ */
window.addOption = (btn) => {
    const optionsWrap = btn.closest(".quiz-question-item").querySelector(".question-options");
    const index = optionsWrap.children.length;

    optionsWrap.insertAdjacentHTML("beforeend", `
        <div class="input-group mb-2">
            <span class="input-group-text">${String.fromCharCode(65 + index)}</span>
            <input class="form-control option-text">
            <span class="input-group-text">
                <input type="checkbox" class="form-check-input option-correct">
            </span>
        </div>
    `);
};

/* ============================================================
   LƯU QUIZ (TẠO HOẶC UPDATE)
============================================================ */
window.saveQuiz = async () => {
    if (!window.currentQuizEdit) return;

    const description = document.getElementById("quizDescription").value.trim();

    // ---- Lưu câu hỏi ----
    const questions = [...document.querySelectorAll(".quiz-question-item")].map(q => {
        const id = q.dataset.qId || null;
        const content = q.querySelector(".question-content").value.trim();

        const options = [...q.querySelectorAll(".input-group")].map(opt => ({
            id: opt.querySelector(".option-text").dataset.optId || null,
            text: opt.querySelector(".option-text").value.trim(),
            isCorrect: opt.querySelector(".option-correct").checked
        }));

        return { id, content, options };
    });

    // ---- Payload ----
    const payload = {
        title: "",      // BE sẽ tự tạo "Quiz {id}"
        description,
        type: 3,
        questions
    };

    try {
        if (window.currentQuizEdit.isNew) {
            // Tạo quiz mới
            await QuizApi.createQuiz({
                ...payload,
                lessonId: window.currentQuizEdit.lessonId
            });

            Toast.show("Tạo Quiz thành công!", "success");
        } else {
            // Update quiz
            await QuizApi.updateQuiz(window.currentQuizEdit.quizId, payload);
            Toast.show("Cập nhật Quiz thành công!", "success");
        }

        // Đóng modal
        bootstrap.Modal.getInstance(document.getElementById("editQuizModal")).hide();

    } catch (err) {
        console.error(err);
        Toast.show("Lưu Quiz thất bại!", "danger");
    }
};

/* ============================================================
   MỞ MODAL SỬA TỪ DANH SÁCH BÀI HỌC
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
            "",     // không dùng title
            quiz,
            false
        );
    } catch (err) {
        console.error(err);
        Toast.show("Không tải được Quiz!", "danger");
    }
};
