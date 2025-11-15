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
        Task<SubjectResponse> CreateAsync(CreateSubjectRequest request, int? userId = null);
        Task<SubjectResponse?> UpdateAsync(int id, UpdateSubjectRequest request, int? userId = null);
        Task<bool> DeleteAsync(int id, int? userId = null);
        Task<SubjectResponse?> GetByIdAsync(int id);
        Task<List<SubjectResponse>> GetAllAsync();

        // Soft delete và restore
        Task<List<SubjectResponse>> GetDeletedSubjectsAsync();
        //Task<bool> RestoreAsync(int id);
    }
}
