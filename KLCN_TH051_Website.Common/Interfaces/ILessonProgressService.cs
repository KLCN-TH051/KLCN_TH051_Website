using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ILessonProgressService
    {
        Task<LessonProgressResponse> UpdateProgressAsync(int enrollmentId, UpdateLessonProgressRequest request);
        Task<List<LessonProgressResponse>> GetProgressByEnrollmentAsync(int enrollmentId);
    }
}
