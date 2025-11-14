using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IKhoaHocService
    {
        Task<KhoaHocResponse> CreateKhoaHocAsync(KhoaHocRequest request, string teacherId);
        Task<IEnumerable<KhoaHocResponse>> GetAllKhoaHocAsync();
        Task<KhoaHocResponse?> GetKhoaHocByIdAsync(int id);
    }
}
