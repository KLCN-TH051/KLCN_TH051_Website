using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateProfileRequest
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; }
    }
}
