using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateCourseRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public int SubjectId { get; set; }        // Khóa học thuộc môn nào
        public CoursesStatus Status { get; set; } = CoursesStatus.Pending; // Trạng thái
    }
}
