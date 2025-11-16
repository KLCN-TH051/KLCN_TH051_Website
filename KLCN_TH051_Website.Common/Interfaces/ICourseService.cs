using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Interfaces
{
    public interface ICourseService
    {
        // CRUD
        // 1. Tạo khóa học Draft (Name + SubjectId)
        Task<CourseResponse> CreateDraftCourseAsync(string name, int subjectId, string creatorId);

        // 2. Cập nhật chi tiết khóa học
        Task<CourseResponse> UpdateCourseAsync(int id, UpdateCourseRequest request);

        // 3. Gửi khóa học từ Draft → Pending
        Task<CourseResponse> SubmitCourseAsync(int id);

        // 4. Admin duyệt hoặc từ chối khóa học
        Task<CourseResponse> UpdateCourseStatusAsync(int id, CoursesStatus status);

        // 5. Xóa khóa học
        Task<bool> DeleteCourseAsync(int id);

        // 6. Lấy khóa học theo Id
        Task<CourseResponse?> GetCourseByIdAsync(int id);

        // 7. Lấy tất cả khóa học (Admin/Teacher)
        Task<List<CourseResponse>> GetAllCoursesAsync();

        // 8. Lấy các khóa học đã duyệt (Student view)
        Task<List<CourseResponse>> GetApprovedCoursesAsync();
    }
}
