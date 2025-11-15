using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Answer: BaseEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }     
        public int OrderNumber { get; set; }
    }
}
