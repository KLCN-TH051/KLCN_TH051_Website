using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Enums;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KLCN_TH051_Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // -----------------------------
        [HttpGet("teacher")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<List<CourseResponse>>> GetTeacherCourses()
        {
            // Lấy teacherId từ JWT token
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int teacherId))
            {
                return Unauthorized("Không lấy được TeacherId từ token");
            }

            var courses = await _courseService.GetCoursesByTeacherAsync(teacherId);
            return Ok(courses);
        }
        // -----------------------------
        // 1. Giáo viên tạo khóa học Draft
        // -----------------------------
        [HttpPost("draft")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateDraft([FromBody] CreateCourseRequest request)
        {
            var userId = User?.Identity?.Name; // Hoặc lấy Id từ Claims
            var course = await _courseService.CreateDraftCourseAsync(request.Name, request.SubjectId, userId!);
            return Ok(course);
        }

        // -----------------------------
        // 2. Cập nhật chi tiết khóa học
        // -----------------------------
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseRequest request)
        {
            var course = await _courseService.UpdateCourseAsync(id, request);
            return Ok(course);
        }

        // -----------------------------
        // 3. Gửi khóa học Draft → Pending
        // -----------------------------
        [HttpPost("{id}/submit")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Submit(int id)
        {
            var course = await _courseService.SubmitCourseAsync(id);
            return Ok(course);
        }

        // -----------------------------
        // 4. Admin duyệt/từ chối khóa học
        // -----------------------------
        [HttpPost("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] CoursesStatus status)
        {
            var course = await _courseService.UpdateCourseStatusAsync(id, status);
            return Ok(course);
        }

        // -----------------------------
        // 5. Xóa khóa học
        // -----------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // -----------------------------
        // 6. Lấy khóa học theo Id
        // -----------------------------
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        // -----------------------------
        // 7. Lấy tất cả khóa học (Admin/Teacher)
        // -----------------------------
        [HttpGet("all")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        // -----------------------------
        // 8. Lấy khóa học đã duyệt (Student)
        // -----------------------------
        [HttpGet("approved")]
        [Authorize(Roles = "Student,Teacher,Admin")]
        public async Task<IActionResult> GetApproved()
        {
            var courses = await _courseService.GetApprovedCoursesAsync();
            return Ok(courses);
        }
    }
}
