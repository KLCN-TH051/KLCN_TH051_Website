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
    public class CourseRatingService : ICourseRatingService
    {
        private readonly AppDbContext _context;

        public CourseRatingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CourseRatingResponse> CreateRatingAsync(CreateCourseRatingRequest request)
        {
            var existing = await _context.CourseRatings
                .FirstOrDefaultAsync(r => r.StudentId == request.StudentId && r.CourseId == request.CourseId);

            if (existing != null)
                throw new Exception("Student has already rated this course");

            var rating = new CourseRating
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            _context.CourseRatings.Add(rating);
            await _context.SaveChangesAsync();

            return new CourseRatingResponse(rating);
        }

        public async Task<CourseRatingResponse> UpdateRatingAsync(int id, UpdateCourseRatingRequest request)
        {
            var rating = await _context.CourseRatings.FindAsync(id);
            if (rating == null)
                throw new Exception("Rating not found");

            rating.Rating = request.Rating ?? rating.Rating;
            rating.Comment = request.Comment ?? rating.Comment;

            await _context.SaveChangesAsync();

            return new CourseRatingResponse(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            var rating = await _context.CourseRatings.FindAsync(id);
            if (rating == null)
                throw new Exception("Rating not found");

            _context.CourseRatings.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CourseRatingResponse>> GetRatingsByCourseAsync(int courseId)
        {
            var ratings = await _context.CourseRatings
                .Where(r => r.CourseId == courseId)
                .ToListAsync();

            return ratings.Select(r => new CourseRatingResponse(r)).ToList();
        }

        public async Task<CourseRatingResponse> GetRatingByIdAsync(int id)
        {
            var rating = await _context.CourseRatings.FindAsync(id);
            if (rating == null)
                throw new Exception("Rating not found");

            return new CourseRatingResponse(rating);
        }

    }
}
