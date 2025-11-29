//// wwwroot/js/modules/instructor/quiz/quiz.questions.js
//import QuestionApi from "/js/api/questionApi.js";
//import Toast from "/js/components/Toast.js";

//window.addQuestion = async () => {
//    if (!window.currentQuizEdit || !window.currentQuizEdit.quizId) {
//        Toast.show("Quiz chưa được lưu, không thể thêm câu hỏi!", "warning");
//        return;
//    }

//    const quizId = window.currentQuizEdit.quizId;
//    const newQuestionData = {
//        quizId,
//        questionText: "Câu hỏi mới",
//        points: 0
//    };

//    try {
//        const question = await QuestionApi.createQuestion(newQuestionData);

//        if (!window.currentQuizEdit.questions) window.currentQuizEdit.questions = [];
//        window.currentQuizEdit.questions.push({
//            id: question.id,
//            content: question.questionText,
//            points: question.points,
//            answers: []
//        });

//        renderQuestions();
//        Toast.show("Thêm câu hỏi thành công!", "success");
//    } catch (err) {
//        console.error(err);
//        Toast.show("Thêm câu hỏi thất bại!", "danger");
//    }
//};

//window.deleteQuestion = async (qIndex) => {
//    if (!window.currentQuizEdit || !window.currentQuizEdit.questions) return;
//    const question = window.currentQuizEdit.questions[qIndex];

//    try {
//        if (question.id) {
//            await QuestionApi.deleteQuestion(question.id);
//        }

//        window.currentQuizEdit.questions.splice(qIndex, 1);
//        renderQuestions();
//        Toast.show("Xóa câu hỏi thành công!", "success");
//    } catch (err) {
//        console.error(err);
//        Toast.show("Xóa câu hỏi thất bại!", "danger");
//    }
//};

//window.addAnswer = (qIndex) => {
//    const question = window.currentQuizEdit.questions[qIndex];
//    if (!question.answers) question.answers = [];

//    question.answers.push({
//        id: null,
//        content: "",
//        isCorrect: false
//    });

//    renderQuestions();
//};

//window.deleteAnswer = (qIndex, aIndex) => {
//    const question = window.currentQuizEdit.questions[qIndex];
//    if (!question || !question.answers) return;

//    question.answers.splice(aIndex, 1);
//    renderQuestions();
//};

//function renderQuestions() {
//    const container = document.getElementById("quizQuestionList");
//    if (!window.currentQuizEdit || !window.currentQuizEdit.questions) return;
//    const questions = window.currentQuizEdit.questions;

//    container.innerHTML = questions.map((q, index) => `
//        <div class="quiz-question mb-3" data-index="${index}">
//            <div class="d-flex justify-content-between align-items-center mb-1">
//                <strong>Câu hỏi ${index + 1}</strong>
//                <button type="button" class="btn btn-sm btn-outline-danger" onclick="deleteQuestion(${index})">Xóa câu hỏi</button>
//            </div>
//            <textarea class="form-control mb-2 question-content" placeholder="Nhập câu hỏi...">${q.content || ''}</textarea>
//            <div class="answers-list">
//                ${q.answers.map((a, i) => `
//                    <div class="d-flex align-items-center mb-1 answer-item" data-index="${i}">
//                        <input type="radio" name="correct-${index}" class="form-check-input me-2" ${a.isCorrect ? 'checked' : ''} onclick="setCorrectAnswer(${index}, ${i})">
//                        <input type="text" class="form-control me-2 answer-content" value="${a.content || ''}" placeholder="Nhập đáp án" oninput="updateAnswerContent(${index}, ${i}, this.value)">
//                        <button type="button" class="btn btn-sm btn-outline-danger" onclick="deleteAnswer(${index}, ${i})">Xóa</button>
//                    </div>
//                `).join('')}
//            </div>
//            <button type="button" class="btn btn-sm btn-primary mt-1" onclick="addAnswer(${index})">Thêm đáp án</button>
//            <hr>
//        </div>
//    `).join('');
//}

//window.setCorrectAnswer = (qIndex, aIndex) => {
//    const question = window.currentQuizEdit.questions[qIndex];
//    question.answers.forEach((a, i) => a.isCorrect = i === aIndex);
//    renderQuestions();
//};

//window.updateAnswerContent = (qIndex, aIndex, value) => {
//    const question = window.currentQuizEdit.questions[qIndex];
//    question.answers[aIndex].content = value;
//};
