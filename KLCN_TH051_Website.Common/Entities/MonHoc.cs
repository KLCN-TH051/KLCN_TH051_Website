using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class MonHoc : BaseEntity
    {
        public int MonHocId { get; set; }
        public string TenMon { get; set; }
        public string MoTa { get; set; }

        // Người tạo là Admin
        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        // Một môn học có thể có nhiều khóa học
        public ICollection<KhoaHoc> KhoaHocs { get; set; }
    }
}
