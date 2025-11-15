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
        public DateTime CreatedDate { get; set; }

        // Constructor nhận entity
        public SubjectResponse(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            Description = subject.Description;
            CreatedDate = subject.CreatedDate;
        }
    }
}
