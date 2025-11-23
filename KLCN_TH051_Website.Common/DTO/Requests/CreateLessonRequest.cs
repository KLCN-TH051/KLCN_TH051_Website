using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public  class CreateLessonRequest
    {
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsFree { get; set; } = false;
        public LessonType Type { get; set; }
    }
}
