using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
     public class QuizAttempt: BaseEntity
    {
        public int Id { get; set; }           // PK

        public int StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int QuizId { get; set; }       // FK → Quiz
        public Quiz Quiz { get; set; }

        public int Score { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.UtcNow;
        public bool Passed { get; set; }
        public int AttemptNumber { get; set; }
        public string Answers { get; set; }      // JSON lưu câu trả lời
    }
}
