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
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        // Tạo môn học
        public async Task<SubjectResponse> CreateAsync(CreateSubjectRequest request, int? adminUserId = null)
        {
            // Kiểm tra trùng tên
            bool exists = await _context.Subjects
                .AnyAsync(s => !s.IsDeleted && s.Name.ToLower() == request.Name.Trim().ToLower());
            if (exists)
                throw new InvalidOperationException("Môn học đã tồn tại.");

            var subject = new Subject
            {
                Name = request.Name.Trim(),
                Description = request.Description,
                CreatedByUserId = adminUserId,
                CreatedBy = adminUserId?.ToString(),
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new SubjectResponse(subject);
        }

        // Cập nhật môn học
        public async Task<SubjectResponse?> UpdateAsync(int id, UpdateSubjectRequest request, int? adminUserId = null)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null || subject.IsDeleted) return null;

            // Kiểm tra trùng tên nếu có thay đổi
            if (!string.IsNullOrWhiteSpace(request.Name) &&
                request.Name.Trim().ToLower() != subject.Name.ToLower())
            {
                bool exists = await _context.Subjects
                    .AnyAsync(s => !s.IsDeleted && s.Name.ToLower() == request.Name.Trim().ToLower() && s.Id != id);
                if (exists)
                    throw new InvalidOperationException("Môn học đã tồn tại.");

                subject.Name = request.Name.Trim();
            }

            if (request.Description != null)
                subject.Description = request.Description;

            subject.LastUpdatedBy = adminUserId?.ToString();
            subject.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return new SubjectResponse(subject);
        }


        // Soft delete
        public async Task<bool> DeleteAsync(int id, int? adminUserId = null)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null || subject.IsDeleted) return false;

            subject.IsDeleted = true;
            subject.DeletedBy = adminUserId?.ToString();
            subject.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // Lấy môn học theo id (không lấy bản ghi đã xóa)
        public async Task<SubjectResponse?> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (subject == null) return null;
            return new SubjectResponse(subject);
        }

        // Lấy tất cả môn học (không lấy bản ghi đã xóa)
        public async Task<List<SubjectResponse>> GetAllAsync()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Courses)
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return subjects.Select(s => new SubjectResponse(s)).ToList();
        }

        // Lấy danh sách các môn đã bị soft delete
        public async Task<List<SubjectResponse>> GetDeletedSubjectsAsync()
        {
            var deletedSubjects = await _context.Subjects
                .Where(s => s.IsDeleted)
                .Include(s => s.Courses)
                .ToListAsync();

            return deletedSubjects.Select(s => new SubjectResponse(s)).ToList();
        }

        // Restore môn học đã xóa
        //public async Task<bool> RestoreAsync(int id)
        //{
        //    var subject = await _context.Subjects.FindAsync(id);
        //    if (subject == null || !subject.IsDeleted) return false;

        //    subject.IsDeleted = false;
        //    subject.DeletedTime = null;
        //    subject.LastUpdatedDate = DateTime.Now;

        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
