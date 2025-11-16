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
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsFree { get; set; } = false;
    }
}
