using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class AnswerResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int OrderNumber { get; set; }

        public AnswerResponse(Answer answer)
        {
            Id = answer.Id;
            QuestionId = answer.QuestionId;
            AnswerText = answer.AnswerText;
            IsCorrect = answer.IsCorrect;
            OrderNumber = answer.OrderNumber;
        }
    }
}
