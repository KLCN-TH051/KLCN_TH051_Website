using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class ChapterResponse
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }

        public ChapterResponse(Chapter chapter)
        {
            Id = chapter.Id;
            Name = chapter.Name;
            Order = chapter.Order;
        }
    }
}
