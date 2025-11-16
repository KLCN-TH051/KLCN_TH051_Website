using KLCN_TH051_Website.Common.Enums;
using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class ContentBlockResponse
    {
        public int Id { get; set; }
        public ContentType Type { get; set; }
        public string? TextContent { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageCaption { get; set; }
        public string? Format { get; set; }
        public int Order { get; set; }

        public ContentBlockResponse(ContentBlock block)
        {
            Id = block.Id;
            Type = block.Type;
            TextContent = block.TextContent;
            ImageUrl = block.ImageUrl;
            ImageCaption = block.ImageCaption;
            Format = block.Format;
            Order = block.Order;
        }
    }
}
