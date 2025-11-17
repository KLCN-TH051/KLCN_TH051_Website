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
    public class LessonProgressService : ILessonProgressService
    {
        private readonly AppDbContext _context;

        public LessonProgressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LessonProgressResponse> UpdateProgressAsync(int enrollmentId, UpdateLessonProgressRequest request)
        {
            // lấy enrollment
            var enrollment = await _context.Enrollments
                .Include(e => e.LessonProgresses)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null)
                throw new Exception("Enrollment not found");

            // tìm progress bài học
            var progress = enrollment.LessonProgresses
                .FirstOrDefault(lp => lp.LessonId == request.LessonId);

            // nếu chưa có thì tạo
            if (progress == null)
            {
                progress = new LessonProgress
                {
                    EnrollmentId = enrollment.Id,
                    LessonId = request.LessonId,
                    WatchTimeSeconds = 0,
                    IsCompleted = false
                };

                _context.LessonProgresses.Add(progress);
            }

            // cập nhật số giây xem — giữ nguyên logic tổng
            progress.WatchTimeSeconds = request.WatchTimeSeconds;
            progress.LastWatchedDate = DateTime.UtcNow;

            // đánh dấu hoàn thành
            if (request.IsCompleted == true)
            {
                progress.IsCompleted = true;
                progress.CompletedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // cập nhật % tiến độ khóa học
            await UpdateEnrollmentProgressAsync(enrollmentId);

            return new LessonProgressResponse(progress);
        }

        public async Task<List<LessonProgressResponse>> GetProgressByEnrollmentAsync(int enrollmentId)
        {
            var list = await _context.LessonProgresses
                .Where(lp => lp.EnrollmentId == enrollmentId)
                .ToListAsync();

            return list.Select(lp => new LessonProgressResponse(lp)).ToList();
        }

        private async Task UpdateEnrollmentProgressAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.LessonProgresses)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null)
                return;

            // tổng lesson của khóa học
            int totalLessons = await _context.Lessons
                .Where(l => l.Chapter.CourseId == enrollment.CourseId && !l.IsDeleted)
                .CountAsync();

            // số đã hoàn thành
            int completedLessons = enrollment.LessonProgresses
                .Count(lp => lp.IsCompleted);

            // tỷ lệ %
            enrollment.ProgressPercentage =
                totalLessons == 0 ? 0 : (completedLessons * 100f / totalLessons);

            // cập nhật trạng thái enrollment
            if (enrollment.ProgressPercentage >= 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
