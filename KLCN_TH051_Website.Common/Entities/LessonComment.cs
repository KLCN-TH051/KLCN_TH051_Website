using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class LessonComment: BaseEntity 
    {
        public int Id { get; set; }

        // FK tới Student (ApplicationUser)
        public int StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        // FK tới Lesson
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        // Nội dung comment
        public string Comment { get; set; }

        // Comment cha (nếu là reply)
        public string? ParentCommentId { get; set; }
        public LessonComment? ParentComment { get; set; }
        public ICollection<LessonComment> Replies { get; set; } = new List<LessonComment>();
    }
}
