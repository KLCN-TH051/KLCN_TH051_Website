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
    public class BannerService : IBannerService
    {
        private readonly AppDbContext _context;

        public BannerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BannerResponse> CreateAsync(CreateBannerRequest request)
        {
            // Tự động tăng Order
            int maxOrder = await _context.Banners.MaxAsync(b => (int?)b.Order) ?? 0;

            var banner = new Banner
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                LinkUrl = request.LinkUrl,
                Order = maxOrder + 1,
                IsActive = true
            };

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return new BannerResponse(banner);
        }

        public async Task<BannerResponse> UpdateAsync(int id, UpdateBannerRequest request)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) throw new Exception("Banner not found");

            banner.Title = request.Title;
            banner.ImageUrl = request.ImageUrl;
            banner.LinkUrl = request.LinkUrl;
            banner.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return new BannerResponse(banner);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return false;

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<BannerResponse>> GetAllAsync()
        {
            return await _context.Banners
                .OrderBy(b => b.Order)
                .Select(b => new BannerResponse(b))
                .ToListAsync();
        }

        public async Task<bool> ReorderAsync(int id, int newOrder)
        {
            var banners = await _context.Banners.OrderBy(b => b.Order).ToListAsync();
            var banner = banners.FirstOrDefault(b => b.Id == id);

            if (banner == null) return false;

            // Xóa khỏi danh sách
            banners.Remove(banner);

            // Chèn vào vị trí mới
            banners.Insert(newOrder - 1, banner);

            // Cập nhật lại thứ tự
            for (int i = 0; i < banners.Count; i++)
            {
                banners[i].Order = i + 1;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
