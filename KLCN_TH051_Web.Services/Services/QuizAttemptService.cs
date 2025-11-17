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
    public class QuizAttemptService : IQuizAttemptService
    {
        private readonly AppDbContext _context;

        public QuizAttemptService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuizAttemptResponse> CreateQuizAttemptAsync(CreateQuizAttemptRequest request)
        {
            var quiz = await _context.Quizzes.FindAsync(request.QuizId);
            if (quiz == null || quiz.IsDeleted)
                throw new Exception("Quiz not found");

            var attempt = new QuizAttempt
            {
                StudentId = request.StudentId,
                QuizId = request.QuizId,
                Score = request.Score,
                Passed = request.Passed,
                AttemptNumber = request.AttemptNumber,
                Answers = request.Answers,
                AttemptDate = DateTime.UtcNow
            };

            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            return new QuizAttemptResponse(attempt);
        }

        public async Task<QuizAttemptResponse> UpdateQuizAttemptAsync(int id, UpdateQuizAttemptRequest request)
        {
            var attempt = await _context.QuizAttempts.FindAsync(id);
            if (attempt == null)
                throw new Exception("Quiz attempt not found");

            attempt.Score = request.Score;
            attempt.Passed = request.Passed;
            attempt.Answers = request.Answers;

            await _context.SaveChangesAsync();

            return new QuizAttemptResponse(attempt);
        }

        public async Task<QuizAttemptResponse> GetQuizAttemptByIdAsync(int id)
        {
            var attempt = await _context.QuizAttempts.FindAsync(id);
            if (attempt == null)
                throw new Exception("Quiz attempt not found");

            return new QuizAttemptResponse(attempt);
        }

        public async Task<List<QuizAttemptResponse>> GetAttemptsByQuizAsync(int quizId, int studentId)
        {
            var list = await _context.QuizAttempts
                .Where(a => a.QuizId == quizId && a.StudentId == studentId)
                .OrderByDescending(a => a.AttemptDate)
                .ToListAsync();

            return list.Select(a => new QuizAttemptResponse(a)).ToList();
        }
    }
}
