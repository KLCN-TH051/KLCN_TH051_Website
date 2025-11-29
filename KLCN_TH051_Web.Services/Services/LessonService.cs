using KLCN_TH051_Web.Repositories.Data;
using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Web.Services.Services
{
    public class LessonService : ILessonService
    {
        private readonly AppDbContext _context;

        public LessonService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LessonResponse> CreateLessonAsync(CreateLessonRequest request, int chapterId, string createdBy)
        {
            // Lấy số thứ tự lớn nhất hiện có trong chapter
            var maxOrder = await _context.Lessons
                .Where(l => l.ChapterId == chapterId && !l.IsDeleted)
                .MaxAsync(l => (int?)l.OrderNumber) ?? 0;

            var lesson = new Lesson
            {
                Title = request.Title,
                Type = request.Type,
                IsFree = request.IsFree,
                ChapterId = chapterId,
                OrderNumber = maxOrder + 1, // ✅ tự động tăng
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                IsDeleted = false
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return new LessonResponse(lesson);
        }



        public async Task<LessonResponse> UpdateLessonContentAsync(int lessonId, UpdateLessonContentRequest request, string updatedBy)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) throw new Exception("Lesson not found");
            if (lesson.Type != LessonType.Content) throw new Exception("Lesson type is not Content");

            lesson.Title = request.Title ?? lesson.Title;
            lesson.Content = request.Content ?? lesson.Content;
            lesson.IsFree = request.IsFree ?? lesson.IsFree;
            //lesson.OrderNumber = request.OrderNumber ?? lesson.OrderNumber;

            lesson.LastUpdatedDate = DateTime.Now;
            lesson.LastUpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return new LessonResponse(lesson);
        }

        public async Task<LessonResponse> UpdateLessonVideoAsync(int lessonId, UpdateLessonVideoRequest request, string updatedBy)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) throw new Exception("Lesson not found");
            if (lesson.Type != LessonType.Video) throw new Exception("Lesson type is not Video");

            lesson.Title = request.Title ?? lesson.Title;
            lesson.VideoUrl = request.VideoUrl ?? lesson.VideoUrl;
            lesson.IsFree = request.IsFree ?? lesson.IsFree;
            lesson.DurationMinutes = request.DurationMinutes ?? lesson.DurationMinutes;
            //lesson.OrderNumber = request.OrderNumber ?? lesson.OrderNumber;

            lesson.LastUpdatedDate = DateTime.Now;
            lesson.LastUpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return new LessonResponse(lesson);
        }

        public async Task<LessonResponse> UpdateLessonQuizAsync(int lessonId, UpdateLessonQuizRequest request, string updatedBy)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) throw new Exception("Lesson not found");
            if (lesson.Type != LessonType.Quiz) throw new Exception("Lesson type is not Quiz");

            lesson.Title = request.Title ?? lesson.Title;
            lesson.IsFree = request.IsFree ?? lesson.IsFree;
            lesson.DurationMinutes = request.DurationMinutes ?? lesson.DurationMinutes;

            lesson.LastUpdatedDate = DateTime.Now;
            lesson.LastUpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return new LessonResponse(lesson);
        }


        public async Task<bool> DeleteLessonAsync(int lessonId, string deletedBy)
        {
            var lesson = await _context.Lessons.FindAsync(lessonId);
            if (lesson == null) return false;

            int deletedOrder = lesson.OrderNumber;
            int chapterId = lesson.ChapterId;

            // Xóa lesson
            lesson.IsDeleted = true;
            lesson.DeletedTime = DateTime.Now;
            lesson.DeletedBy = deletedBy;

            await _context.SaveChangesAsync();

            // Reorder các lesson còn lại
            var remainingLessons = await _context.Lessons
                .Where(l => l.ChapterId == chapterId && !l.IsDeleted && l.OrderNumber > deletedOrder)
                .ToListAsync();

            foreach (var l in remainingLessons)
            {
                l.OrderNumber--; // Giảm order number đi 1
            }

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<LessonResponse> GetLessonByIdAsync(int lessonId)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Chapter)
                .FirstOrDefaultAsync(l => l.Id == lessonId && !l.IsDeleted);

            if (lesson == null) return null;

            return new LessonResponse(lesson);
        }

        public async Task<IEnumerable<LessonResponse>> GetLessonsByChapterAsync(int chapterId)
        {
            var lessons = await _context.Lessons
                .Where(l => l.ChapterId == chapterId && !l.IsDeleted)
                .OrderBy(l => l.OrderNumber)
                .ToListAsync();

            return lessons.Select(l => new LessonResponse(l));
        }
    }
}
