using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Requests
{
    public class UpdateAccountRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }       // Nếu muốn admin đổi role
        public bool? IsActive { get; set; }     // Kích hoạt / khóa tài khoản
        public string? Avatar { get; set; }     // URL ảnh đại diện
    }
}
