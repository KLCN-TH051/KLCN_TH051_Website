using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; } // tùy chọn
        public List<string> Permissions { get; set; }
    }
}
