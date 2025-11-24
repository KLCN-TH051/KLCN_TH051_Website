using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateLessonContentRequest
    {
        public string? Title { get; set; }       // optional, có thể đổi tên
        public bool? IsFree { get; set; }        // optional
        public string Content { get; set; }      // bắt buộc nếu type = Content
        public int? OrderNumber { get; set; }     // optional
    }
}
