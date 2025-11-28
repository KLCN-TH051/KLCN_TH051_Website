using KLCN_TH051_Web.Repositories.Data;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Services.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request, string creatorId)
        {
            var quiz = await _context.Quizzes.FindAsync(request.QuizId);
            if (quiz == null || quiz.IsDeleted)
                throw new Exception("Quiz not found");

            // Tự động OrderNumber
            int maxOrder = await _context.Questions
                .Where(q => q.QuizId == request.QuizId)
                .MaxAsync(q => (int?)q.OrderNumber) ?? 0;

            var question = new Question
            {
                QuizId = request.QuizId,
                QuestionText = request.QuestionText,
                Points = request.Points,
                OrderNumber = maxOrder + 1,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return new QuestionResponse(question);
        }

        public async Task<List<QuestionResponse>> GetQuestionsByQuizAsync(int quizId)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId && !q.IsDeleted)
                .OrderBy(q => q.OrderNumber)
                .ToListAsync();

            return questions.Select(q => new QuestionResponse(q)).ToList();
        }

        public async Task<QuestionResponse> GetQuestionByIdAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null || question.IsDeleted)
                throw new Exception("Question not found");

            return new QuestionResponse(question);
        }

        public async Task<QuestionResponse> UpdateQuestionAsync(int id, UpdateQuestionRequest request, string updaterId)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null || question.IsDeleted)
                throw new Exception("Question not found");

            question.QuestionText = request.QuestionText ?? question.QuestionText;
            question.Points = request.Points ?? question.Points;

            question.LastUpdatedBy = updaterId;
            question.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return new QuestionResponse(question);
        }

        public async Task DeleteQuestionAsync(int id, string deleterId)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null || question.IsDeleted)
                throw new Exception("Question not found");

            question.IsDeleted = true;
            question.DeletedBy = deleterId;
            question.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task ReorderQuestionsAsync(int quizId, List<int> questionIdsInNewOrder)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId && questionIdsInNewOrder.Contains(q.Id))
                .ToListAsync();

            if (questions.Count != questionIdsInNewOrder.Count)
                throw new Exception("Invalid question Ids");

            for (int i = 0; i < questionIdsInNewOrder.Count; i++)
            {
                var question = questions.First(q => q.Id == questionIdsInNewOrder[i]);
                question.OrderNumber = i + 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<QuestionResponse>> CreateManyAsync(List<CreateQuestionRequest> requests, string creatorId)
        {
            var responses = new List<QuestionResponse>();

            foreach (var request in requests)
            {
                var quiz = await _context.Quizzes.FindAsync(request.QuizId);
                if (quiz == null || quiz.IsDeleted)
                    throw new Exception($"Quiz {request.QuizId} not found");

                // Tự động OrderNumber
                int maxOrder = await _context.Questions
                    .Where(q => q.QuizId == request.QuizId)
                    .MaxAsync(q => (int?)q.OrderNumber) ?? 0;

                var question = new Question
                {
                    QuizId = request.QuizId,
                    QuestionText = request.QuestionText,
                    Points = request.Points,
                    OrderNumber = maxOrder + 1,
                    CreatedBy = creatorId,
                    CreatedDate = DateTime.Now
                };

                _context.Questions.Add(question);
                responses.Add(new QuestionResponse(question));
            }

            await _context.SaveChangesAsync();
            return responses;
        }

    }
}
