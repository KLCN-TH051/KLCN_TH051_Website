using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Entities
{
    public interface IBaseEntity
    {
        string? CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string? LastUpdatedBy { get; set; }
        DateTime? LastUpdatedDate { get; set; }
        string? DeletedBy { get; set; }
        DateTime? DeletedTime { get; set; }
        bool IsDeleted { get; set; }
    }
}
