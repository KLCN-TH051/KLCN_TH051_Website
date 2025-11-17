using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class QuizAttemptResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public bool Passed { get; set; }
        public int AttemptNumber { get; set; }
        public string Answers { get; set; }
        public DateTime AttemptDate { get; set; }

        public QuizAttemptResponse() { }

        public QuizAttemptResponse(QuizAttempt attempt)
        {
            Id = attempt.Id;
            StudentId = attempt.StudentId;
            QuizId = attempt.QuizId;
            Score = attempt.Score;
            Passed = attempt.Passed;
            AttemptNumber = attempt.AttemptNumber;
            Answers = attempt.Answers;
            AttemptDate = attempt.AttemptDate;
        }
    }
}
