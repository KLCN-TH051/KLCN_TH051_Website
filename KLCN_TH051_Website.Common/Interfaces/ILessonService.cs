using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ILessonService
    {
        // chapterId được truyền trực tiếp vào service, không nằm trong request
        Task<LessonResponse> CreateLessonAsync(CreateLessonRequest request, int chapterId, string createdBy);

        Task<LessonResponse> UpdateLessonContentAsync(int lessonId, UpdateLessonContentRequest request, string updatedBy);
        Task<LessonResponse> UpdateLessonVideoAsync(int lessonId, UpdateLessonVideoRequest request, string updatedBy);
        Task<LessonResponse> UpdateLessonQuizAsync(int lessonId, UpdateLessonQuizRequest request, string updatedBy);
        Task<bool> DeleteLessonAsync(int lessonId, string deletedBy);
        Task<LessonResponse> GetLessonByIdAsync(int lessonId);
        Task<IEnumerable<LessonResponse>> GetLessonsByChapterAsync(int chapterId);
    }

}
