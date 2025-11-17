using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class UserWithRoleResponse
    {
        public int Id { get; set; }                   // int, theo migration
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Avatar { get; set; }          // đặt trùng tên migration
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
