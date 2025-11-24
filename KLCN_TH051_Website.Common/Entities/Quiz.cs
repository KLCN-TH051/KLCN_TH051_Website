using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Quiz: BaseEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }     // FK → Lesson
        public Lesson Lesson { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public QuizType Type { get; set; } = QuizType.SingleChoice;         // SingleChoice hoặc MultipleChoice
        public int PassingScore { get; set; } = 70;
        public int TimeLimitMinutes { get; set; } = 30;
        public int? MaxAttempts { get; set; }

        public ICollection<Question>? Questions { get; set; }
        public ICollection<QuizAttempt>? Attempts { get; set; }
    }
}
