using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IAnswerService
    {
        Task<AnswerResponse> CreateAnswerAsync(CreateAnswerRequest request);
        Task<List<AnswerResponse>> CreateManyAnswersAsync(List<CreateAnswerRequest> requests);
        Task<AnswerResponse> UpdateAnswerAsync(int id, UpdateAnswerRequest request);
        Task<bool> DeleteAnswerAsync(int id);
        Task<AnswerResponse?> GetAnswerByIdAsync(int id);
        Task<List<AnswerResponse>> GetAnswersByQuestionAsync(int questionId);
    }
}
