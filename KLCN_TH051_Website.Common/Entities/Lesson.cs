using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Lesson: BaseEntity
    {
        public int Id { get; set; }           // PK
        public string Title { get; set; }
        //public string Description { get; set; }
        public int OrderNumber { get; set; }     // Thứ tự bài học trong Chapter
        public int DurationMinutes { get; set; } // Thời lượng ước tính
        public int ChapterId { get; set; }    // FK
        public Chapter Chapter { get; set; }     // Navigation property
        public bool IsFree { get; set; } = false;

        // Trường xác định loại bài học
        public LessonType Type { get; set; }

        // Một lesson có nhiều nội dung
        public ICollection<LessonProgress>? LessonProgresses { get; set; }
        public ICollection<Quiz>? Quizzes{ get; set; }
        public ICollection<LessonComment>? LessonComments { get; set; }
    }
}
