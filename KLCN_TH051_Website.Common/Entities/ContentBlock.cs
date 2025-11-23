using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class ContentBlock : BaseEntity
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public ContentType Type { get; set; } // Text hoặc Image
        public string? TextContent { get; set; } // Nếu Type = Text
        public string? ImageUrl { get; set; }    // Nếu Type = Image
        public int Order { get; set; } = 0; // Thứ tự hiển thị
    }
}
