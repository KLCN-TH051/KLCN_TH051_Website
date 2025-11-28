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
    public class AnswerService : IAnswerService
    {
        private readonly AppDbContext _context;

        public AnswerService(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // CREATE
        // ============================
        public async Task<AnswerResponse> CreateAnswerAsync(CreateAnswerRequest request)
        {
            // Tự động đánh số thứ tự cho Answer trong câu hỏi
            var count = await _context.Answers
                .Where(a => a.QuestionId == request.QuestionId)
                .CountAsync();

            var answer = new Answer
            {
                QuestionId = request.QuestionId,
                AnswerText = request.AnswerText,
                IsCorrect = request.IsCorrect,
                OrderNumber = count + 1
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return new AnswerResponse(answer);
        }

        // ============================
        // CREATE MULTIPLE ANSWERS
        // ============================
        public async Task<List<AnswerResponse>> CreateManyAnswersAsync(List<CreateAnswerRequest> requests)
        {
            var responses = new List<AnswerResponse>();

            // Nhóm theo QuestionId để đánh thứ tự đúng
            var grouped = requests.GroupBy(r => r.QuestionId);

            foreach (var group in grouped)
            {
                int count = await _context.Answers
                    .Where(a => a.QuestionId == group.Key)
                    .CountAsync();

                foreach (var req in group)
                {
                    count++;
                    var answer = new Answer
                    {
                        QuestionId = req.QuestionId,
                        AnswerText = req.AnswerText,
                        IsCorrect = req.IsCorrect,
                        OrderNumber = count
                    };
                    _context.Answers.Add(answer);
                    responses.Add(new AnswerResponse(answer));
                }
            }

            await _context.SaveChangesAsync();
            return responses;
        }

        // ============================
        // UPDATE
        // ============================
        public async Task<AnswerResponse> UpdateAnswerAsync(int id, UpdateAnswerRequest request)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
                throw new Exception("Answer not found");

            if (!string.IsNullOrEmpty(request.AnswerText))
                answer.AnswerText = request.AnswerText;

            if (request.IsCorrect.HasValue)
                answer.IsCorrect = request.IsCorrect.Value;

            await _context.SaveChangesAsync();

            return new AnswerResponse(answer);
        }

        // ============================
        // DELETE
        // ============================
        public async Task<bool> DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return false;

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }

        // ============================
        // GET BY ID
        // ============================
        public async Task<AnswerResponse?> GetAnswerByIdAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return null;

            return new AnswerResponse(answer);
        }

        // ============================
        // GET BY QUESTION
        // ============================
        public async Task<List<AnswerResponse>> GetAnswersByQuestionAsync(int questionId)
        {
            var answers = await _context.Answers
                .Where(a => a.QuestionId == questionId)
                .OrderBy(a => a.OrderNumber)
                .ToListAsync();

            return answers.Select(a => new AnswerResponse(a)).ToList();
        }
    }
}
