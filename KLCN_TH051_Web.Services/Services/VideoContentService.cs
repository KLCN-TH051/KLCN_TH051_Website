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
    public class VideoContentService : IVideoContentService
    {
        private readonly AppDbContext _context;

        public VideoContentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VideoContentResponse> CreateVideoContentAsync(CreateVideoContentRequest request, string creatorId)
        {
            var lesson = await _context.Lessons.FindAsync(request.LessonId);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            // Tạo video mới
            var video = new VideoContent
            {
                LessonId = request.LessonId,
                VideoUrl = request.VideoUrl,
                DurationSeconds = request.DurationSeconds,
                Quality = request.Quality,
                Subtitle = request.Subtitle,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now
            };

            _context.VideoContents.Add(video);
            await _context.SaveChangesAsync();

            return new VideoContentResponse(video);
        }

        public async Task<List<VideoContentResponse>> GetVideoContentsByLessonAsync(int lessonId)
        {
            var videos = await _context.VideoContents
                .Where(v => v.LessonId == lessonId && !v.IsDeleted)
                .OrderBy(v => v.Id) // hoặc thêm trường Order nếu muốn custom thứ tự
                .ToListAsync();

            return videos.Select(v => new VideoContentResponse(v)).ToList();
        }

        public async Task<VideoContentResponse> GetVideoContentByIdAsync(int id)
        {
            var video = await _context.VideoContents.FindAsync(id);
            if (video == null || video.IsDeleted)
                throw new Exception("Video content not found");

            return new VideoContentResponse(video);
        }

        public async Task<VideoContentResponse> UpdateVideoContentAsync(int id, UpdateVideoContentRequest request, string updaterId)
        {
            var video = await _context.VideoContents.FindAsync(id);
            if (video == null || video.IsDeleted)
                throw new Exception("Video content not found");

            // Update các trường nếu client có gửi
            video.VideoUrl = request.VideoUrl ?? video.VideoUrl;
            video.DurationSeconds = request.DurationSeconds ?? video.DurationSeconds;
            video.Quality = request.Quality ?? video.Quality;
            video.Subtitle = request.Subtitle ?? video.Subtitle;

            video.LastUpdatedBy = updaterId;
            video.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new VideoContentResponse(video);
        }

        public async Task DeleteVideoContentAsync(int id, string deleterId)
        {
            var video = await _context.VideoContents.FindAsync(id);
            if (video == null || video.IsDeleted)
                throw new Exception("Video content not found");

            video.IsDeleted = true;
            video.DeletedBy = deleterId;
            video.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

    }
}
