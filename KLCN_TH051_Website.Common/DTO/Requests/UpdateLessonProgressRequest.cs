using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateLessonProgressRequest
    {
        public int LessonId { get; set; }
        public int WatchTimeSeconds { get; set; }      // số giây FE gửi lên
        public bool? IsCompleted { get; set; }
    }
}
