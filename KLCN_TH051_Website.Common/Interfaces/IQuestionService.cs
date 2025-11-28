using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IQuestionService
    {
        Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request, string creatorId);

        Task<List<QuestionResponse>> GetQuestionsByQuizAsync(int quizId);

        Task<QuestionResponse> GetQuestionByIdAsync(int id);

        Task<QuestionResponse> UpdateQuestionAsync(int id, UpdateQuestionRequest request, string updaterId);

        Task DeleteQuestionAsync(int id, string deleterId);

        Task ReorderQuestionsAsync(int quizId, List<int> questionIdsInNewOrder);

        Task<List<QuestionResponse>> CreateManyAsync(List<CreateQuestionRequest> requests, string creatorId);

    }
}
