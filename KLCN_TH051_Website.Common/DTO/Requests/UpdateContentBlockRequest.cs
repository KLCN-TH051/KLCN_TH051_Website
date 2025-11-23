using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateContentBlockRequest
    {
        public ContentType? Type { get; set; }          
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public int? Order { get; set; }
    }
}
