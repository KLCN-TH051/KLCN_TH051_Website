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

        public async Task<SubjectResponse> CreateAsync(CreateSubjectRequest request, int? adminUserId = null)
        {
            var subject = new Subject
            {
                Name = request.Name,
                Description = request.Description,
                CreatedByUserId = adminUserId
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new SubjectResponse(subject);
        }

        public async Task<SubjectResponse?> UpdateAsync(int id, UpdateSubjectRequest request)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return null;

            if (!string.IsNullOrWhiteSpace(request.Name))
                subject.Name = request.Name;

            if (request.Description != null)
                subject.Description = request.Description;

            await _context.SaveChangesAsync();
            return new SubjectResponse(subject);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<SubjectResponse?> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Courses) // nếu muốn load courses luôn
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null) return null;
            return new SubjectResponse(subject);
        }

        public async Task<List<SubjectResponse>> GetAllAsync()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Courses)
                .ToListAsync();

            return subjects.Select(s => new SubjectResponse(s)).ToList();
        }
    }
}
