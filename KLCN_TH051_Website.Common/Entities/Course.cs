using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Course : BaseEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }       // KH001, KH002

        public string Name { get; set; }

        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        // Ngày khóa học bắt đầu
        public DateTime? StartDate { get; set; }

        // Ngày khóa học kết thúc
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }     // Giá khóa học
        // Liên kết với môn học
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }   // Navigation property

        // Người tạo là giáo viên
        public int? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }
        //Trạng thái khóa học
        public CoursesStatus Status { get; set; } = CoursesStatus.Draft;

        // Navigation property: một khóa học có nhiều Chapter
        public ICollection<Chapter>? Chapters { get; set; }
        // Navigation property: một khóa học có nhiều Enrollment
        public ICollection<Enrollment>? Enrollments { get; set; }
        // Navigation property: một khóa học có nhiều CourseRating
        public ICollection<CourseRating>? CourseRatings { get; set; }
        // Navigation property: một khóa học có nhiều Payment
        public ICollection<Payment>? Payments { get; set; }
    }
}
