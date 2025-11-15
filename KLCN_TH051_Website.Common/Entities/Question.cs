using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Question: BaseEntity
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public string QuestionText { get; set; }
        public decimal Points { get; set; }
        public int OrderNumber { get; set; }

        // 1 Question có nhiều Answer
        public ICollection<Answer>? Answers { get; set; }
    }
}
