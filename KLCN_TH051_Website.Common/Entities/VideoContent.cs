using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class VideoContent: BaseEntity
    {
        public int Id { get; set; }            // PK
        public int LessonId { get; set; }      // FK → Lesson
        public Lesson Lesson { get; set; }        // Navigation

        public string VideoUrl { get; set; }      // Đường dẫn video
        public int DurationSeconds { get; set; }  // Thời lượng video
        public string Quality { get; set; }       // VD: "720p", "1080p"
        public string? Subtitle { get; set; }     // File subtitle (.vtt/.srt) nếu có
    }
}
