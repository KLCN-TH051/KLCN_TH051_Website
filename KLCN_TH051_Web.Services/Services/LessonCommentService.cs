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
    public class LessonCommentService : ILessonCommentService
    {
        private readonly AppDbContext _context;

        public LessonCommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LessonCommentResponse> CreateCommentAsync(CreateLessonCommentRequest request)
        {
            var lesson = await _context.Lessons.FindAsync(request.LessonId);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            var comment = new LessonComment
            {
                StudentId = request.StudentId,
                LessonId = request.LessonId,
                Comment = request.Comment,
                ParentCommentId = request.ParentCommentId
            };

            _context.LessonComments.Add(comment);
            await _context.SaveChangesAsync();

            return new LessonCommentResponse(comment);
        }

        public async Task<LessonCommentResponse> UpdateCommentAsync(int id, UpdateLessonCommentRequest request)
        {
            var comment = await _context.LessonComments.FindAsync(id);
            if (comment == null)
                throw new Exception("Comment not found");

            comment.Comment = request.Comment;
            comment.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new LessonCommentResponse(comment);
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _context.LessonComments.FindAsync(id);
            if (comment == null)
                throw new Exception("Comment not found");

            comment.IsDeleted = true;
            comment.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<List<LessonCommentResponse>> GetCommentsByLessonAsync(int lessonId)
        {
            var comments = await _context.LessonComments
                .Where(c => c.LessonId == lessonId && c.ParentCommentId == null && !c.IsDeleted)
                .Include(c => c.Replies)
                .ToListAsync();

            return comments.Select(c => new LessonCommentResponse(c)).ToList();
        }

        public async Task<LessonCommentResponse> GetCommentByIdAsync(int id)
        {
            var comment = await _context.LessonComments
                .Include(c => c.Replies)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (comment == null)
                throw new Exception("Comment not found");

            return new LessonCommentResponse(comment);
        }
    }
}
