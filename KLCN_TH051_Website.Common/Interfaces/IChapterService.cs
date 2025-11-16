using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IChapterService
    {
        // Tạo chapter (Order tự động)
        Task<ChapterResponse> CreateChapterAsync(int courseId, CreateChapterRequest request);

        // Lấy danh sách chapter theo khóa học
        Task<List<ChapterResponse>> GetChaptersByCourseAsync(int courseId);

        // Lấy chapter theo Id
        Task<ChapterResponse> GetChapterByIdAsync(int id);

        // Cập nhật chapter (chỉ Name)
        Task<ChapterResponse> UpdateChapterAsync(int id, UpdateChapterRequest request);

        // Xóa chapter + reorder
        Task DeleteChapterAsync(int id);

        // Tùy chọn: đổi thứ tự chapter theo danh sách Id
        Task ReorderChaptersAsync(int courseId, List<int> chapterIdsInNewOrder);
    }
}
