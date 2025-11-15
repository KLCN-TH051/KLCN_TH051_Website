using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public  class ApplicationUser : IdentityUser<int>, IBaseEntity
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Ảnh đại diện (URL hoặc đường dẫn ảnh)
        public string? Avatar { get; set; }

        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<CourseRating>? CourseRatings { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}
