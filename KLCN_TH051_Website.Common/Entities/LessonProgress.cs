using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class LessonProgress: BaseEntity
    {
        public int  Id { get; set; }           // PK

        public int  EnrollmentId { get; set; } // FK → Enrollment
        public Enrollment Enrollment { get; set; }

        public int LessonId { get; set; }     // FK → Lesson
        public Lesson Lesson { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedDate { get; set; }

        public int WatchTimeSeconds { get; set; } = 0;
        public DateTime? LastWatchedDate { get; set; }
    }
}
