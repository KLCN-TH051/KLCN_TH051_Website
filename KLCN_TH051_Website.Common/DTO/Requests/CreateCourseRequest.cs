using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateCourseRequest
    {
        public string Name { get; set; }
        public int SubjectId { get; set; }
        //public CoursesStatus Status { get; set; } = CoursesStatus.Draft; 
    }
}
