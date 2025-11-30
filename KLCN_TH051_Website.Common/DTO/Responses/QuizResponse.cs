using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class QuizResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string? Description { get; set; }
        public QuizType Type { get; set; }
        public int PassingScore { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int? MaxAttempts { get; set; }

        public QuizResponse(Quiz quiz)
        {
            Id = quiz.Id;
            LessonId = quiz.LessonId;
            Description = quiz.Description;
            Type = quiz.Type;
            PassingScore = quiz.PassingScore;
            TimeLimitMinutes = quiz.TimeLimitMinutes;
            MaxAttempts = quiz.MaxAttempts;
        }
    }
}
