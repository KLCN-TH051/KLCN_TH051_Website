using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class LessonCommentResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public string Comment { get; set; }
        public string? ParentCommentId { get; set; }
        public List<LessonCommentResponse> Replies { get; set; } = new List<LessonCommentResponse>();

        public LessonCommentResponse(LessonComment comment)
        {
            Id = comment.Id;
            StudentId = comment.StudentId;
            LessonId = comment.LessonId;
            Comment = comment.Comment;
            ParentCommentId = comment.ParentCommentId;

            if (comment.Replies != null && comment.Replies.Any())
                Replies = comment.Replies.Select(r => new LessonCommentResponse(r)).ToList();
        }
    }
}
