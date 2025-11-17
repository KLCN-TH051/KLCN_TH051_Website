using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateCourseRatingRequest
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Rating { get; set; }          // 1-5
        public string? Comment { get; set; }     // Không bắt buộc
    }
}
