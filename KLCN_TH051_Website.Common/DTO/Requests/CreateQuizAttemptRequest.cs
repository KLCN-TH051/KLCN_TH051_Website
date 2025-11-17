using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateQuizAttemptRequest
    {
        public int StudentId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public bool Passed { get; set; }
        public int AttemptNumber { get; set; }
        public string Answers { get; set; } // JSON string lưu câu trả lời
    }
}
