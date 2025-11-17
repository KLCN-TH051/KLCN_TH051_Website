using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ICourseRatingService
    {
        Task<CourseRatingResponse> CreateRatingAsync(CreateCourseRatingRequest request);
        Task<CourseRatingResponse> UpdateRatingAsync(int id, UpdateCourseRatingRequest request);
        Task DeleteRatingAsync(int id);
        Task<List<CourseRatingResponse>> GetRatingsByCourseAsync(int courseId);
        Task<CourseRatingResponse> GetRatingByIdAsync(int id);
    }
}
