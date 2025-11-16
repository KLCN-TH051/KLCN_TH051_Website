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
    public class EnrollmentService : IEnrollmentService
    {
        private readonly AppDbContext _context;

        public EnrollmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EnrollmentResponse> CreateEnrollmentAsync(CreateEnrollmentRequest request)
        {
            // Kiểm tra học viên đã đăng ký khóa học chưa
            var exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == request.StudentId && e.CourseId == request.CourseId && e.Status == EnrollmentStatus.Active);
            if (exists)
                throw new Exception("Student already enrolled in this course");

            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                Status = EnrollmentStatus.Active,
                EnrolledDate = DateTime.Now
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new EnrollmentResponse(enrollment);
        }

        public async Task<EnrollmentResponse> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                throw new Exception("Enrollment not found");

            return new EnrollmentResponse(enrollment);
        }

        public async Task<List<EnrollmentResponse>> GetEnrollmentsByStudentAsync(int studentId)
        {
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
            return enrollments.Select(e => new EnrollmentResponse(e)).ToList();
        }

        public async Task<EnrollmentResponse> UpdateEnrollmentAsync(int id, UpdateEnrollmentRequest request)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                throw new Exception("Enrollment not found");

            enrollment.Status = request.Status ?? enrollment.Status;
            enrollment.ProgressPercentage = request.ProgressPercentage ?? enrollment.ProgressPercentage;

            await _context.SaveChangesAsync();
            return new EnrollmentResponse(enrollment);
        }

        public async Task DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                throw new Exception("Enrollment not found");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}
