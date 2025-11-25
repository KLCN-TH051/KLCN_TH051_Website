using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateLessonVideoRequest
    {
        public string? Title { get; set; }
        public bool? IsFree { get; set; }
        public string VideoUrl { get; set; }      // bắt buộc nếu type = Video
        public int? DurationMinutes { get; set; } // optional
        //public int? OrderNumber { get; set; }     // optional
    }
}
