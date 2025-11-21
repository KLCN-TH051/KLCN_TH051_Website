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
    public class TeacherAssignmentService : ITeacherAssignmentService
    {
        private readonly AppDbContext _context;

        public TeacherAssignmentService(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------------
        // 1. Lấy tất cả phân công giáo viên
        // -----------------------------------
        public async Task<List<TeacherAssignmentResponse>> GetAllAsync()
        {
            return await _context.TeacherAssignments
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .Select(t => new TeacherAssignmentResponse
                {
                    Id = t.Id,
                    TeacherId = t.TeacherId,
                    TeacherName = t.Teacher.FullName,
                    SubjectId = t.SubjectId,
                    SubjectName = t.Subject.Name
                })
                .ToListAsync();
        }

        // -----------------------------------
        // 2. Lấy danh sách môn học của 1 giáo viên
        // -----------------------------------
        public async Task<List<TeacherSubjectResponse>> GetSubjectsByTeacherAsync(int teacherId)
        {
            return await _context.TeacherAssignments
                .Where(t => t.TeacherId == teacherId && t.SubjectId != null)
                .Include(t => t.Subject)
                .Select(t => new TeacherSubjectResponse
                {
                    SubjectId = t.SubjectId,
                    SubjectName = t.Subject.Name ?? "Chưa có tên môn học"
                })
                .ToListAsync();
        }

        // -----------------------------------
        // 3. Tạo phân công giáo viên
        // -----------------------------------
        public async Task<TeacherAssignmentResponse> CreateAsync(CreateTeacherAssignmentRequest request)
        {
            // Kiểm tra phân công trùng
            var exists = await _context.TeacherAssignments
                .AnyAsync(x => x.TeacherId == request.TeacherId && x.SubjectId == request.SubjectId);

            if (exists)
                throw new Exception("Giáo viên đã được phân công môn này.");

            var entity = new TeacherAssignment
            {
                TeacherId = request.TeacherId,
                SubjectId = request.SubjectId
            };

            _context.TeacherAssignments.Add(entity);
            await _context.SaveChangesAsync();

            var teacher = await _context.Users.FindAsync(request.TeacherId);
            var subject = await _context.Subjects.FindAsync(request.SubjectId);

            return new TeacherAssignmentResponse
            {
                Id = entity.Id,
                TeacherId = request.TeacherId,
                TeacherName = teacher?.FullName,
                SubjectId = request.SubjectId,
                SubjectName = subject?.Name
            };
        }

        // -----------------------------------
        // 4. Update phân công giáo viên
        // -----------------------------------
        public async Task<TeacherAssignmentResponse> UpdateAsync(int id, UpdateTeacherAssignmentRequest request)
        {
            var entity = await _context.TeacherAssignments.FindAsync(id);
            if (entity == null)
                throw new Exception("Phân công không tồn tại.");

            // Kiểm tra trùng
            var exists = await _context.TeacherAssignments
                .AnyAsync(x => x.Id != id && x.TeacherId == request.TeacherId && x.SubjectId == request.SubjectId);
            if (exists)
                throw new Exception("Phân công trùng với bản ghi khác.");

            entity.TeacherId = request.TeacherId;
            entity.SubjectId = request.SubjectId;

            await _context.SaveChangesAsync();

            var teacher = await _context.Users.FindAsync(request.TeacherId);
            var subject = await _context.Subjects.FindAsync(request.SubjectId);

            return new TeacherAssignmentResponse
            {
                Id = entity.Id,
                TeacherId = request.TeacherId,
                TeacherName = teacher?.FullName,
                SubjectId = request.SubjectId,
                SubjectName = subject?.Name
            };
        }

        // -----------------------------------
        // 5. Xóa phân công giáo viên
        // -----------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.TeacherAssignments.FindAsync(id);
            if (entity == null)
                return false;

            _context.TeacherAssignments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        // -----------------------------------
        // 6. Lấy phân công theo Id
        // -----------------------------------
        public async Task<TeacherAssignmentResponse> GetByIdAsync(int id)
        {
            var entity = await _context.TeacherAssignments
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null)
                throw new Exception("Phân công không tồn tại.");

            return new TeacherAssignmentResponse
            {
                Id = entity.Id,
                TeacherId = entity.TeacherId,
                TeacherName = entity.Teacher?.FullName,
                SubjectId = entity.SubjectId,
                SubjectName = entity.Subject?.Name
            };
        }

    }
}
