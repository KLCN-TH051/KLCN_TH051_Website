using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Banner : BaseEntity
    {
        public int Id { get; set; }

        // Tiêu đề banner
        public string Title { get; set; }

        // URL hình ảnh banner
        public string ImageUrl { get; set; }

        // Link khi click vào banner
        public string? LinkUrl { get; set; }

        // Thứ tự hiển thị (nếu có nhiều banner)
        public int Order { get; set; }

        // Trạng thái banner: Active / Inactive
        public bool IsActive { get; set; } = true;
    }
}
