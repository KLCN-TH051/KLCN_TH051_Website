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
    public class QuizService : IQuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request, string creatorId)
        {
            var lesson = await _context.Lessons.FindAsync(request.LessonId);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            var quiz = new Quiz
            {
                LessonId = request.LessonId,
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                PassingScore = request.PassingScore,
                TimeLimitMinutes = request.TimeLimitMinutes,
                MaxAttempts = request.MaxAttempts,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return new QuizResponse(quiz);
        }

        public async Task<List<QuizResponse>> GetQuizzesByLessonAsync(int lessonId)
        {
            var quizzes = await _context.Quizzes
                .Where(q => q.LessonId == lessonId && !q.IsDeleted)
                .OrderBy(q => q.Id)
                .ToListAsync();

            return quizzes.Select(q => new QuizResponse(q)).ToList();
        }

        public async Task<QuizResponse> GetQuizByIdAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null || quiz.IsDeleted)
                throw new Exception("Quiz not found");

            return new QuizResponse(quiz);
        }

        public async Task<QuizResponse> UpdateQuizAsync(int id, UpdateQuizRequest request, string updaterId)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null || quiz.IsDeleted)
                throw new Exception("Quiz not found");

            quiz.Title = request.Title ?? quiz.Title;
            quiz.Description = request.Description ?? quiz.Description;
            quiz.Type = request.Type ?? quiz.Type;
            quiz.PassingScore = request.PassingScore ?? quiz.PassingScore;
            quiz.TimeLimitMinutes = request.TimeLimitMinutes ?? quiz.TimeLimitMinutes;
            quiz.MaxAttempts = request.MaxAttempts ?? quiz.MaxAttempts;

            quiz.LastUpdatedBy = updaterId;
            quiz.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new QuizResponse(quiz);
        }

        public async Task DeleteQuizAsync(int id, string deleterId)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null || quiz.IsDeleted)
                throw new Exception("Quiz not found");

            quiz.IsDeleted = true;
            quiz.DeletedBy = deleterId;
            quiz.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }
    }
}
