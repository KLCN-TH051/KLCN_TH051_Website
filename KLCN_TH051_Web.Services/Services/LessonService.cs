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
    public class LessonService : ILessonService
    {
        private readonly AppDbContext _context;

        public LessonService(AppDbContext context)
        {
            _context = context;
        }

        // Tạo lesson mới
        public async Task<LessonResponse> CreateLessonAsync(int chapterId, CreateLessonRequest request, string creatorId)
        {
            // Tìm chapter
            var chapter = await _context.Chapters.FindAsync(chapterId);
            if (chapter == null)
                throw new Exception("Chapter not found");

            // Tự động xác định OrderNumber
            int maxOrder = await _context.Lessons
                .Where(l => l.ChapterId == chapterId && !l.IsDeleted)
                .MaxAsync(l => (int?)l.OrderNumber) ?? 0;

            var lesson = new Lesson
            {
                ChapterId = chapterId,
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                IsFree = request.IsFree,
                Type = request.Type,
                OrderNumber = maxOrder + 1,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return new LessonResponse(lesson);
        }

        // Lấy danh sách lesson theo chapter
        public async Task<List<LessonResponse>> GetLessonsByChapterAsync(int chapterId, bool includeDeleted = false)
        {
            var query = _context.Lessons.Where(l => l.ChapterId == chapterId);
            if (!includeDeleted)
                query = query.Where(l => !l.IsDeleted);

            var lessons = await query.OrderBy(l => l.OrderNumber).ToListAsync();
            return lessons.Select(l => new LessonResponse(l)).ToList();
        }

        // Lấy chi tiết lesson
        public async Task<LessonResponse> GetLessonByIdAsync(int id, bool includeDeleted = false)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null || (!includeDeleted && lesson.IsDeleted))
                throw new Exception("Lesson not found");

            return new LessonResponse(lesson);
        }

        // Cập nhật lesson
        public async Task<LessonResponse> UpdateLessonAsync(int id, UpdateLessonRequest request, string updaterId)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            if (!string.IsNullOrEmpty(request.Title))
                lesson.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Description))
                lesson.Description = request.Description;

            if (request.DurationMinutes.HasValue)
                lesson.DurationMinutes = request.DurationMinutes.Value;

            if (request.IsFree.HasValue)
                lesson.IsFree = request.IsFree.Value;

            // Cập nhật loại bài học nếu có
            if (request.Type.HasValue)
                lesson.Type = request.Type.Value;

            lesson.LastUpdatedBy = updaterId;
            lesson.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new LessonResponse(lesson);
        }

        // Xóa lesson (soft delete)
        public async Task DeleteLessonAsync(int id, string deleterId)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            lesson.IsDeleted = true;
            lesson.DeletedBy = deleterId;
            lesson.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        // Reorder lessons
        public async Task ReorderLessonsAsync(int chapterId, List<int> lessonIdsInNewOrder)
        {
            var lessons = await _context.Lessons
                .Where(l => l.ChapterId == chapterId && lessonIdsInNewOrder.Contains(l.Id))
                .ToListAsync();

            if (lessons.Count != lessonIdsInNewOrder.Count)
                throw new Exception("Invalid lesson Ids");

            for (int i = 0; i < lessonIdsInNewOrder.Count; i++)
            {
                var lesson = lessons.First(l => l.Id == lessonIdsInNewOrder[i]);
                lesson.OrderNumber = i + 1;
            }

            await _context.SaveChangesAsync();
        }

    }
}
