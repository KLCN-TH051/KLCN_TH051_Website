using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class CourseRatingResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public CourseRatingResponse(CourseRating rating)
        {
            Id = rating.Id;
            StudentId = rating.StudentId;
            CourseId = rating.CourseId;
            Rating = rating.Rating;
            Comment = rating.Comment;
        }
    }
}
