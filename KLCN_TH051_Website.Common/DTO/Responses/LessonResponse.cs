using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class LessonResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public LessonType Type { get; set; }
        public bool IsFree { get; set; }
        public int OrderNumber { get; set; }
        public int DurationMinutes { get; set; }
        public int ChapterId { get; set; }

        public string? Content { get; set; }
        public string? VideoUrl { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Constructor mapping từ entity Lesson
        public LessonResponse(Lesson lesson)
        {
            if (lesson == null) throw new ArgumentNullException(nameof(lesson));

            Id = lesson.Id;
            Title = lesson.Title;
            Type = lesson.Type;
            IsFree = lesson.IsFree;
            OrderNumber = lesson.OrderNumber;
            DurationMinutes = lesson.DurationMinutes;
            ChapterId = lesson.ChapterId;

            // Chỉ map dữ liệu theo type
            Content = lesson.Type == LessonType.Content ? lesson.Content : null;
            VideoUrl = lesson.Type == LessonType.Video ? lesson.VideoUrl : null;

            CreatedDate = lesson.CreatedDate;
            LastUpdatedDate = lesson.LastUpdatedDate;
            IsDeleted = lesson.IsDeleted;
        }
    }
}
