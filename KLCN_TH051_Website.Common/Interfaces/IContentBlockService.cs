using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface IContentBlockService
    {
        // Tạo content block mới (Order tự động)
        Task<ContentBlockResponse> CreateContentBlockAsync(CreateContentBlockRequest request, string creatorId);

        // Lấy danh sách content block theo lesson
        Task<List<ContentBlockResponse>> GetContentBlocksByLessonAsync(int lessonId);

        // Lấy chi tiết content block
        Task<ContentBlockResponse> GetContentBlockByIdAsync(int id);

        // Cập nhật content block
        Task<ContentBlockResponse> UpdateContentBlockAsync(int id, UpdateContentBlockRequest request, string updaterId);

        // Xóa content block (soft delete)
        Task DeleteContentBlockAsync(int id, string deleterId);

        // Thay đổi thứ tự hiển thị (reorder)
        Task ReorderContentBlocksAsync(int lessonId, List<int> contentBlockIdsInNewOrder);
    }
}
