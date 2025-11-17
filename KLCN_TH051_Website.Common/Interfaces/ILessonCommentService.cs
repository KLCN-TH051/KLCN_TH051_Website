using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ILessonCommentService
    {
        Task<LessonCommentResponse> CreateCommentAsync(CreateLessonCommentRequest request);
        Task<LessonCommentResponse> UpdateCommentAsync(int id, UpdateLessonCommentRequest request);
        Task DeleteCommentAsync(int id);
        Task<List<LessonCommentResponse>> GetCommentsByLessonAsync(int lessonId);
        Task<LessonCommentResponse> GetCommentByIdAsync(int id);
    }
}
