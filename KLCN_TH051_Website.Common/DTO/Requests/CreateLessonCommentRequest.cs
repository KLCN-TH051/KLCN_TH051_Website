using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateLessonCommentRequest
    {
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public string Comment { get; set; }
        public string? ParentCommentId { get; set; } // Nếu là reply
    }
}
