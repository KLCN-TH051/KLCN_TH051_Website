using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IQuizAttemptService
    {
        Task<QuizAttemptResponse> CreateQuizAttemptAsync(CreateQuizAttemptRequest request);
        Task<QuizAttemptResponse> UpdateQuizAttemptAsync(int id, UpdateQuizAttemptRequest request);
        Task<QuizAttemptResponse> GetQuizAttemptByIdAsync(int id);
        Task<List<QuizAttemptResponse>> GetAttemptsByQuizAsync(int quizId, int studentId);
    }
}
