using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Chapter : BaseEntity
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        // Liên kết với khóa học
        public int CourseId { get; set; }     // khóa ngoại
        // Navigation property
        public Course Course { get; set; }   

        // Một chapter có nhiều lesson
        public ICollection<Lesson>? Lessons { get; set; }
    }
}
