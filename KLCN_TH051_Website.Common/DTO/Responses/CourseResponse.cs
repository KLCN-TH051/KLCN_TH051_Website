using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
     public class CourseResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public CoursesStatus Status { get; set; }

        public string TeacherName { get; set; }

        public CourseResponse(Course course)
        {
            Id = course.Id;
            Code = course.Code;
            Name = course.Name;
            Description = course.Description;
            Thumbnail = course.Thumbnail;
            StartDate = course.StartDate;
            EndDate = course.EndDate;
            Price = course.Price;
            SubjectId = course.SubjectId;
            SubjectName = course.Subject?.Name ?? "";
            Status = course.Status;

            TeacherName = course.CreatedByUser != null ? course.CreatedByUser.FullName : "Chưa cập nhật";
        }
    }
}
