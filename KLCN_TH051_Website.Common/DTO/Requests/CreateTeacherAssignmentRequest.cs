using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateTeacherAssignmentRequest
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
    }
}
