using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string QuestionText { get; set; }
        public decimal Points { get; set; }
        public int OrderNumber { get; set; }

        public QuestionResponse(Question question)
        {
            Id = question.Id;
            QuizId = question.QuizId;
            QuestionText = question.QuestionText;
            Points = question.Points;
            OrderNumber = question.OrderNumber;
        }
    }
}
