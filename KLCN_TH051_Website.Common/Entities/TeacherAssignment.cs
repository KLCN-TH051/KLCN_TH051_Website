using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class TeacherAssignment : BaseEntity
    {
            public int Id { get; set; }

            public int TeacherId { get; set; }        // Id từ AspNetUsers
            public ApplicationUser Teacher { get; set; }

            public int SubjectId { get; set; }
            public Subject Subject { get; set; }
        public int? CourseId { get; set; }        // Cho phép null
        public Course? Course { get; set; }
    }
}
