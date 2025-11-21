using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ITeacherAssignmentService
    {
        Task<List<TeacherAssignmentResponse>> GetAllAsync();
        Task<List<TeacherSubjectResponse>> GetSubjectsByTeacherAsync(int teacherId);
        Task<TeacherAssignmentResponse> CreateAsync(CreateTeacherAssignmentRequest request);
        Task<bool> DeleteAsync(int id);

        // Thêm method Update
        Task<TeacherAssignmentResponse> UpdateAsync(int id, UpdateTeacherAssignmentRequest request);

        Task<TeacherAssignmentResponse> GetByIdAsync(int id);
    }
}
