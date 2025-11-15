using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public class Subject : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        // Người tạo là Admin
        public int? CreatedByUserId { get; set; }
        public ApplicationUser? CreatedByUser { get; set; }

        // **Navigation property cần thêm**
        public ICollection<Course>? Courses { get; set; }
    }
}
