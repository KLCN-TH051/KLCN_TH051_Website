using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public  interface ILessonService
    {
        // Tạo lesson mới trong chapter (OrderNumber tự động)
        Task<LessonResponse> CreateLessonAsync(int chapterId, CreateLessonRequest request, string creatorId);

        // Lấy danh sách lesson theo chapter (học sinh không thấy lesson đã xóa)
        Task<List<LessonResponse>> GetLessonsByChapterAsync(int chapterId, bool includeDeleted = false);

        // Lấy chi tiết lesson
        Task<LessonResponse> GetLessonByIdAsync(int id, bool includeDeleted = false);

        // Cập nhật lesson
        Task<LessonResponse> UpdateLessonAsync(int id, UpdateLessonRequest request, string updaterId);

        // Xóa lesson (soft delete)
        Task DeleteLessonAsync(int id, string deleterId);

        // Tuỳ chọn: đổi thứ tự lesson theo danh sách Id
        Task ReorderLessonsAsync(int chapterId, List<int> lessonIdsInNewOrder);
    }
}
