using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateVideoContentRequest
    {
        // LessonId sẽ liên kết video với Lesson tương ứng
        public int LessonId { get; set; }

        // Thông tin video
        public string VideoUrl { get; set; }
        public int DurationSeconds { get; set; }
        public string Quality { get; set; }
        public string? Subtitle { get; set; }      // Optional
    }
}
