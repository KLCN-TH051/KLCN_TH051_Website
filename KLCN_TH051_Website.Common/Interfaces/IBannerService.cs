using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IBannerService
    {
        Task<BannerResponse> CreateAsync(CreateBannerRequest request);
        Task<BannerResponse> UpdateAsync(int id, UpdateBannerRequest request);
        Task<bool> DeleteAsync(int id);
        Task<List<BannerResponse>> GetAllAsync();
        Task<bool> ReorderAsync(int id, int newOrder);
    }
}
