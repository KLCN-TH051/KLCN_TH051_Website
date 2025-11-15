using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        public bool IsDeleted { get; set; }

        public SubjectResponse(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            Description = subject.Description;
            CreatedBy = subject.CreatedBy;
            CreatedDate = subject.CreatedDate;
            LastUpdatedBy = subject.LastUpdatedBy;
            LastUpdatedDate = subject.LastUpdatedDate;
            DeletedBy = subject.DeletedBy;
            DeletedTime = subject.DeletedTime;
            IsDeleted = subject.IsDeleted;
        }
    }
}
