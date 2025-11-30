using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateQuizRequest
    {
        public int LessonId { get; set; }         // Lesson chứa quiz
        public string? Description { get; set; }
        public QuizType Type { get; set; }        // SingleChoice / MultipleChoice
        public int PassingScore { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int? MaxAttempts { get; set; }
    }
}
