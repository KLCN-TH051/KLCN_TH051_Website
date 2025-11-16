using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IQuizService
    {
        // Tạo quiz mới
        Task<QuizResponse> CreateQuizAsync(CreateQuizRequest request, string creatorId);

        // Lấy danh sách quiz theo lesson
        Task<List<QuizResponse>> GetQuizzesByLessonAsync(int lessonId);

        // Lấy chi tiết quiz
        Task<QuizResponse> GetQuizByIdAsync(int id);

        // Cập nhật quiz
        Task<QuizResponse> UpdateQuizAsync(int id, UpdateQuizRequest request, string updaterId);

        // Xóa quiz (soft delete)
        Task DeleteQuizAsync(int id, string deleterId);
    }
}
