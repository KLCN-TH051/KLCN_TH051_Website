using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateQuestionRequest
    {
        public int QuizId { get; set; }                 // Thuộc quiz nào
        public string QuestionText { get; set; }       // Nội dung câu hỏi
        public decimal Points { get; set; }            // Điểm câu hỏi
    }
}
