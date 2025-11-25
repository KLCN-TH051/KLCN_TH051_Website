using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateLessonQuizRequest
    {
        public string? Title { get; set; }
        public bool? IsFree { get; set; }
        public int? DurationMinutes { get; set; } // optional
        //public int? OrderNumber { get; set; }     // optional
    }
}
