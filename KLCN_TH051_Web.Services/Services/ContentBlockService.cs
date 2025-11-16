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
    public class ContentBlockService : IContentBlockService
    {
        private readonly AppDbContext _context;

        public ContentBlockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContentBlockResponse> CreateContentBlockAsync(CreateContentBlockRequest request, string creatorId)
        {
            var lesson = await _context.Lessons.FindAsync(request.LessonId);
            if (lesson == null || lesson.IsDeleted)
                throw new Exception("Lesson not found");

            // ❌ Thêm kiểm tra nghiệp vụ
            if (lesson.Type != LessonType.Content)
                throw new Exception("Cannot create ContentBlock for this type of lesson. Only Content lessons are allowed.");

            // Tự động Order: lấy max Order của lesson +1
            int maxOrder = await _context.ContentBlocks
                .Where(c => c.LessonId == request.LessonId)
                .MaxAsync(c => (int?)c.Order) ?? 0;

            var contentBlock = new ContentBlock
            {
                LessonId = request.LessonId,
                Type = request.Type,
                TextContent = request.TextContent,
                ImageUrl = request.ImageUrl,
                ImageCaption = request.ImageCaption,
                Format = request.Format,
                Order = maxOrder + 1,
                CreatedBy = creatorId,
                CreatedDate = DateTime.Now
            };

            _context.ContentBlocks.Add(contentBlock);
            await _context.SaveChangesAsync();

            return new ContentBlockResponse(contentBlock);
        }


        public async Task<List<ContentBlockResponse>> GetContentBlocksByLessonAsync(int lessonId)
        {
            var blocks = await _context.ContentBlocks
                .Where(c => c.LessonId == lessonId)
                .OrderBy(c => c.Order)
                .ToListAsync();

            return blocks.Select(b => new ContentBlockResponse(b)).ToList();
        }

        public async Task<ContentBlockResponse> GetContentBlockByIdAsync(int id)
        {
            var block = await _context.ContentBlocks.FindAsync(id);
            if (block == null)
                throw new Exception("Content block not found");

            return new ContentBlockResponse(block);
        }

        public async Task<ContentBlockResponse> UpdateContentBlockAsync(int id, UpdateContentBlockRequest request, string updaterId)
        {
            var block = await _context.ContentBlocks.FindAsync(id);
            if (block == null)
                throw new Exception("Content block not found");

            block.Type = request.Type ?? block.Type;
            block.TextContent = request.TextContent ?? block.TextContent;
            block.ImageUrl = request.ImageUrl ?? block.ImageUrl;
            block.ImageCaption = request.ImageCaption ?? block.ImageCaption;
            block.Format = request.Format ?? block.Format;

            block.LastUpdatedBy = updaterId;
            block.LastUpdatedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ContentBlockResponse(block);
        }

        public async Task DeleteContentBlockAsync(int id, string deleterId)
        {
            var block = await _context.ContentBlocks.FindAsync(id);
            if (block == null)
                throw new Exception("Content block not found");

            block.IsDeleted = true;
            block.DeletedBy = deleterId;
            block.DeletedTime = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task ReorderContentBlocksAsync(int lessonId, List<int> contentBlockIdsInNewOrder)
        {
            var blocks = await _context.ContentBlocks
                .Where(c => c.LessonId == lessonId && contentBlockIdsInNewOrder.Contains(c.Id))
                .ToListAsync();

            if (blocks.Count != contentBlockIdsInNewOrder.Count)
                throw new Exception("Invalid content block Ids");

            for (int i = 0; i < contentBlockIdsInNewOrder.Count; i++)
            {
                var block = blocks.First(c => c.Id == contentBlockIdsInNewOrder[i]);
                block.Order = i + 1;
            }

            await _context.SaveChangesAsync();
        }
    }
}
