using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectResponse> CreateAsync(CreateSubjectRequest request, int? adminUserId = null);
        Task<SubjectResponse?> UpdateAsync(int id, UpdateSubjectRequest request);
        Task<bool> DeleteAsync(int id);
        Task<SubjectResponse?> GetByIdAsync(int id);
        Task<List<SubjectResponse>> GetAllAsync();
    }
}
