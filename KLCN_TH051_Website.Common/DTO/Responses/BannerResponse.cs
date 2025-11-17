using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class BannerResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string? LinkUrl { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }

        public BannerResponse(Banner banner)
        {
            Id = banner.Id;
            Title = banner.Title;
            ImageUrl = banner.ImageUrl;
            LinkUrl = banner.LinkUrl;
            Order = banner.Order;
            IsActive = banner.IsActive;
        }
    }
}
