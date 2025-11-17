using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateCourseRatingRequest
    {
        public int? Rating { get; set; }         // Cho phép update một số trường
        public string? Comment { get; set; }
    }
}
