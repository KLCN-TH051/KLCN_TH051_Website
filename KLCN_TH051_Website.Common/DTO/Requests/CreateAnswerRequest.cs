using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateAnswerRequest
    {
        public int QuestionId { get; set; }      // Câu hỏi thuộc về
        public string AnswerText { get; set; }   // Nội dung câu trả lời
        public bool IsCorrect { get; set; }
    }
}
