using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
     public class TeacherAssignmentResponse
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }
        public string TeacherName { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
