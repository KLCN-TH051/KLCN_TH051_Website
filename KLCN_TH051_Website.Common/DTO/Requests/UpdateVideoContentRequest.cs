using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateVideoContentRequest
    {
        // Thông tin video có thể cập nhật
        public string? VideoUrl { get; set; }
        public int? DurationSeconds { get; set; }
        public string? Quality { get; set; }
        public string? Subtitle { get; set; }
    }
}
