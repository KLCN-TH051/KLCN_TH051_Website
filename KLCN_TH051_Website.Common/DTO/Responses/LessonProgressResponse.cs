using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class LessonProgressResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public bool IsCompleted { get; set; }
        public int WatchTimeSeconds { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? LastWatchedDate { get; set; }

        public LessonProgressResponse(LessonProgress p)
        {
            Id = p.Id;
            LessonId = p.LessonId;
            IsCompleted = p.IsCompleted;
            WatchTimeSeconds = p.WatchTimeSeconds;
            CompletedDate = p.CompletedDate;
            LastWatchedDate = p.LastWatchedDate;
        }
    }
}
