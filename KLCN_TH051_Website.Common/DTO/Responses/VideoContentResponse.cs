using KLCN_TH051_Website.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.DTO.Responses
{
    public class VideoContentResponse
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string VideoUrl { get; set; }
        public int DurationSeconds { get; set; }
        public string Quality { get; set; }
        public string? Subtitle { get; set; }

        public VideoContentResponse(VideoContent video)
        {
            Id = video.Id;
            LessonId = video.LessonId;
            VideoUrl = video.VideoUrl;
            DurationSeconds = video.DurationSeconds;
            Quality = video.Quality;
            Subtitle = video.Subtitle;
        }
    }
}
