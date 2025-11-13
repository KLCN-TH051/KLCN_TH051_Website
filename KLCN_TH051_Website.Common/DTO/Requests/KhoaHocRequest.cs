using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class KhoaHocRequest
    {
        public string TenKhoaHoc { get; set; }
        public string MaKhoaHoc { get; set; }
        public string MoTa { get; set; }
        public decimal Gia { get; set; }       
        public int MonHocId { get; set; }      // Liên kết môn học
    }
}
