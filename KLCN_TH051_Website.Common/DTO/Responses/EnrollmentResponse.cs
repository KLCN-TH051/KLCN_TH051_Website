using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class EnrollmentResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public EnrollmentStatus Status { get; set; }
        public float ProgressPercentage { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? LastAccessedDate { get; set; }

        public EnrollmentResponse(Enrollment enrollment)
        {
            Id = enrollment.Id;
            StudentId = enrollment.StudentId;
            CourseId = enrollment.CourseId;
            EnrolledDate = enrollment.EnrolledDate;
            Status = enrollment.Status;
            ProgressPercentage = enrollment.ProgressPercentage;
            CompletedDate = enrollment.CompletedDate;
            LastAccessedDate = enrollment.LastAccessedDate;
        }
    }
}
