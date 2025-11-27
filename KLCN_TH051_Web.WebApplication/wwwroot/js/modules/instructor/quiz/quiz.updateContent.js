import QuizApi from "/js/api/quizApi.js";
import Toast from "/js/components/Toast.js";

window.currentQuizEdit = null;

// === MỞ MODAL QUIZ (tạo mới hoặc sửa) ===
window.openQuizModal = (chapterId, lessonId, title, quiz = null, isNew = false) => {
    window.currentQuizEdit = {
        chapterId: parseInt(chapterId),
        lessonId: parseInt(lessonId),
        quizId: quiz?.id ?? null,
        quizTitle: title || "",
        isNew
    };

    const modalEl = document.getElementById("editQuizModal");
    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);

    // Điền dữ liệu
    document.getElementById("quizTitle").value = quiz?.title || title || "";
    document.getElementById("quizDescription").value = quiz?.description || "";
    document.getElementById("quizPassingScore").value = quiz?.passingScore ?? 0;
    document.getElementById("quizTimeLimit").value = quiz?.timeLimitMinutes ?? 0;
    document.getElementById("quizMaxAttempts").value = quiz?.maxAttempts ?? 0;

    // Tiêu đề modal
    modalEl.querySelector(".modal-title").textContent = isNew ? "Tạo Quiz mới" : "Chỉnh sửa Quiz";

    modal.show();

    // Reset khi đóng
    modalEl.addEventListener("hidden.bs.modal", () => {
        window.currentQuizEdit = null;
        document.getElementById("quizTitle").value = "";
        document.getElementById("quizDescription").value = "";
        document.getElementById("quizPassingScore").value = 0;
        document.getElementById("quizTimeLimit").value = 0;
        document.getElementById("quizMaxAttempts").value = 0;
    }, { once: true });
};

// === LƯU QUIZ ===
document.getElementById("btnSaveQuiz")?.addEventListener("click", async () => {
    if (!window.currentQuizEdit) return;

    const title = document.getElementById("quizTitle").value.trim();
    const description = document.getElementById("quizDescription").value.trim();
    const passingScore = parseInt(document.getElementById("quizPassingScore").value) || 0;
    const timeLimitMinutes = parseInt(document.getElementById("quizTimeLimit").value) || 0;
    const maxAttempts = parseInt(document.getElementById("quizMaxAttempts").value) || 0;

    if (!title) {
        Toast.show("Vui lòng nhập tiêu đề!", "danger");
        return;
    }

    try {
        const payload = {
            title,
            description,
            type: 3, // Quiz
            passingScore,
            timeLimitMinutes,
            maxAttempts
        };

        if (window.currentQuizEdit.isNew) {
            await QuizApi.createQuiz({ ...payload, lessonId: window.currentQuizEdit.lessonId });
            Toast.show("Tạo Quiz thành công!", "success");
        } else {
            await QuizApi.updateQuiz(window.currentQuizEdit.quizId, payload);
            Toast.show("Cập nhật Quiz thành công!", "success");
        }

        bootstrap.Modal.getInstance(document.getElementById("editQuizModal")).hide();

        // Reload danh sách quiz (tùy chọn: render lại lesson list nếu cần)
        const quizzes = await QuizApi.getQuizzesByLesson(window.currentQuizEdit.lessonId);
        if (typeof window.quizListModule?.renderQuizzesIntoLesson === "function") {
            window.quizListModule.renderQuizzesIntoLesson(window.currentQuizEdit.lessonId, quizzes);
        }

        window.currentQuizEdit = null;
    } catch (err) {
        console.error(err);
        Toast.show("Lưu Quiz thất bại!", "danger");
    }
});

// === HỖ TRỢ SỬA QUIZ TỪ NÚT BÚT CHÌ (lesson.list.js gọi) ===
window.editQuiz = async (lessonId) => {
    try {
        const quizzes = await QuizApi.getQuizzesByLesson(lessonId);
        const quiz = quizzes[0]; // nếu 1 lesson chỉ có 1 quiz
        if (!quiz) {
            Toast.show("Quiz chưa được tạo!", "info");
            return;
        }

        window.openQuizModal(quiz.chapterId, lessonId, quiz, false);
    } catch (err) {
        console.error(err);
        Toast.show("Không tải được Quiz!", "danger");
    }
};
