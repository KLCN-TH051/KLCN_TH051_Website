using KLCN_TH051_Website.Common.Entities;
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
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsFree { get; set; }

        public LessonResponse(Lesson lesson)
        {
            Id = lesson.Id;
            Title = lesson.Title;
            Description = lesson.Description;
            OrderNumber = lesson.OrderNumber;
            DurationMinutes = lesson.DurationMinutes;
            IsFree = lesson.IsFree;
        }
    }
}
