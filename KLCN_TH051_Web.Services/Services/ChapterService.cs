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
    public class ChapterService : IChapterService
    {
        private readonly AppDbContext _context;

        public ChapterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ChapterResponse> CreateChapterAsync(int courseId, CreateChapterRequest request)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                throw new Exception("Course not found");

            int nextOrder = await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .CountAsync() + 1;

            var chapter = new Chapter
            {
                Name = request.Name,
                Order = nextOrder,
                CourseId = courseId,
                CreatedDate = DateTime.Now,
            };

            _context.Chapters.Add(chapter);
            await _context.SaveChangesAsync();

            return new ChapterResponse(chapter);
        }

        public async Task<List<ChapterResponse>> GetChaptersByCourseAsync(int courseId)
        {
            return await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.Order)
                .Select(c => new ChapterResponse(c))
                .ToListAsync();
        }

        public async Task<ChapterResponse> GetChapterByIdAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) throw new Exception("Chapter not found");
            return new ChapterResponse(chapter);
        }

        public async Task<ChapterResponse> UpdateChapterAsync(int id, UpdateChapterRequest request)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) throw new Exception("Chapter not found");

            if (!string.IsNullOrEmpty(request.Name))
                chapter.Name = request.Name;

            await _context.SaveChangesAsync();
            return new ChapterResponse(chapter);
        }

        public async Task DeleteChapterAsync(int id)
        {
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null) throw new Exception("Chapter not found");

            int courseId = chapter.CourseId;

            _context.Chapters.Remove(chapter);
            await _context.SaveChangesAsync();

            // Reorder lại các chapter còn lại
            var chapters = await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            for (int i = 0; i < chapters.Count; i++)
            {
                chapters[i].Order = i + 1;
            }

            await _context.SaveChangesAsync();
        }

        public async Task ReorderChaptersAsync(int courseId, List<int> chapterIdsInNewOrder)
        {
            var chapters = await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .ToListAsync();

            if (chapters.Count != chapterIdsInNewOrder.Count)
                throw new Exception("Danh sách chapter không khớp");

            for (int i = 0; i < chapterIdsInNewOrder.Count; i++)
            {
                var chapter = chapters.FirstOrDefault(c => c.Id == chapterIdsInNewOrder[i]);
                if (chapter == null) throw new Exception("Chapter không tồn tại");
                chapter.Order = i + 1;
            }

            await _context.SaveChangesAsync();
        }
    }
}
