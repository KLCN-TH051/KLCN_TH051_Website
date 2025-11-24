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
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        private string GenerateCourseCode()
        {
            return "CRS-" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }

        // 1. Tạo khóa học Draft
        public async Task<CourseResponse> CreateDraftCourseAsync(string name, int subjectId, int creatorUserId)
        {
            var course = new Course
            {
                Code = GenerateCourseCode(),
                Name = name,
                SubjectId = subjectId,
                CreatedByUserId = creatorUserId,
                CreatedDate = DateTime.Now,
                Status = CoursesStatus.Draft
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            await _context.Entry(course).Reference(c => c.CreatedByUser).LoadAsync();

            return new CourseResponse(course);
        }

        // 2. Cập nhật chi tiết khóa học
        public async Task<CourseResponse> UpdateCourseAsync(int id, UpdateCourseRequest request)
        {
            var course = await _context.Courses
                .Include(c => c.CreatedByUser)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Course not found");

            if (!string.IsNullOrEmpty(request.Name))
                course.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Description))
                course.Description = request.Description;

            if (!string.IsNullOrEmpty(request.Thumbnail))
                course.Thumbnail = request.Thumbnail;

            if (request.Price.HasValue)
                course.Price = request.Price.Value;

            if (request.StartDate.HasValue)
                course.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                course.EndDate = request.EndDate.Value;

            course.Status = CoursesStatus.Draft;

            await _context.SaveChangesAsync();

            return new CourseResponse(course);
        }

        // 3. Gửi khóa học Draft → Pending
        public async Task<CourseResponse> SubmitCourseAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.CreatedByUser)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Course not found");

            if (course.Status != CoursesStatus.Draft && course.Status != CoursesStatus.Rejected)
                throw new Exception("Chỉ các khóa học ở trạng thái Draft hoặc Rejected mới được gửi duyệt.");

            course.Status = CoursesStatus.Pending;
            await _context.SaveChangesAsync();

            return new CourseResponse(course);
        }

        // 4. Admin duyệt/từ chối khóa học
        public async Task<CourseResponse> UpdateCourseStatusAsync(int id, CoursesStatus status)
        {
            var course = await _context.Courses
                .Include(c => c.CreatedByUser)
                .Include(c => c.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Course not found");

            if (status != CoursesStatus.Approved && status != CoursesStatus.Rejected)
                throw new Exception("Chỉ Admin mới thay đổi trạng thái thành Approved hoặc Rejected.");

            course.Status = status;
            await _context.SaveChangesAsync();

            return new CourseResponse(course);
        }

        // 5. Xóa khóa học
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }

        // 6. Lấy khóa học theo Id
        public async Task<CourseResponse?> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.CreatedByUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course == null ? null : new CourseResponse(course);
        }

        // 7. Lấy tất cả khóa học (Admin/Teacher)
        public async Task<List<CourseResponse>> GetAllCoursesAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.CreatedByUser)
                .ToListAsync();

            return courses.Select(c => new CourseResponse(c)).ToList();
        }

        // 8. Lấy khóa học đã duyệt (Student view)
        public async Task<List<CourseResponse>> GetApprovedCoursesAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.CreatedByUser)
                .Where(c => c.Status == CoursesStatus.Approved)
                .ToListAsync();

            return courses.Select(c => new CourseResponse(c)).ToList();
        }

        // 9. Lấy khóa học của giáo viên
        public async Task<List<CourseResponse>> GetCoursesByTeacherAsync(int teacherId)
        {
            var courses = await _context.Courses
                .Where(c => c.CreatedByUserId == teacherId)
                .Include(c => c.Subject)
                .Include(c => c.CreatedByUser)
                .ToListAsync();

            return courses.Select(c => new CourseResponse(c)).ToList();
        }
    }
}
