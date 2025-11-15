using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateSubjectRequest
    {
        public string? Name { get; set; }          // ? vì có thể chỉ muốn update 1 trường
        public string? Description { get; set; }
    }
}
