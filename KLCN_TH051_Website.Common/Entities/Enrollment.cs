using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Enrollment: BaseEntity
    {
        public int Id { get; set; }  

        // Học viên
        public int StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        // Khóa học
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Ngày đăng ký
        public DateTime EnrolledDate { get; set; } = DateTime.Now;

        // Trạng thái: Active, Completed, Cancelled
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        // Tiến độ học viên đã học (%)
        public float ProgressPercentage { get; set; } = 0;

        // Ngày hoàn thành khóa học (nếu đã hoàn thành)
        public DateTime? CompletedDate { get; set; }

        // Lần cuối học viên truy cập
        public DateTime? LastAccessedDate { get; set; }

        // Một enrollment có nhiều LessonProgress
        public ICollection<LessonProgress>? LessonProgresses { get; set; }
    }
}
