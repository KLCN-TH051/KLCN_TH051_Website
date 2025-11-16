using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IVideoContentService
    {
        Task<VideoContentResponse> CreateVideoContentAsync(CreateVideoContentRequest request, string creatorId);
        Task<List<VideoContentResponse>> GetVideoContentsByLessonAsync(int lessonId);
        Task<VideoContentResponse> GetVideoContentByIdAsync(int id);
        Task<VideoContentResponse> UpdateVideoContentAsync(int id, UpdateVideoContentRequest request, string updaterId);
        Task DeleteVideoContentAsync(int id, string deleterId);
    }
}
