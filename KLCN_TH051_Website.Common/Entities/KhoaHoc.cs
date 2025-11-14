using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class KhoaHoc : BaseEntity
    {
        public int KhoaHocId { get; set; }

        public string MaKhoaHoc { get; set; } // VD: KH001, KH002

        public string TenKhoaHoc { get; set; }
        public string MoTa { get; set; }
        // Giá khóa học
        public decimal Gia { get; set; }  // Ví dụ 500000 = 500k VNĐ

        // Liên kết với môn học
        public int MonHocId { get; set; }
        public MonHoc MonHoc { get; set; }

        // Người tạo là giáo viên
        public string? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        public KhoaHocStatus Status { get; set; } = KhoaHocStatus.Pending;
    }
}
