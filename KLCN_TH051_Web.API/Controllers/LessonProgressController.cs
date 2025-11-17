using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/enrollments/{enrollmentId}/lesson-progress")]
    [ApiController]
    public class LessonProgressController : ControllerBase
    {
        private readonly ILessonProgressService _lessonProgressService;

        public LessonProgressController(ILessonProgressService lessonProgressService)
        {
            _lessonProgressService = lessonProgressService;
        }

        /// <summary>
        /// Cập nhật tiến độ học 1 bài học (lesson) trong 1 enrollment
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateLessonProgress(
            int enrollmentId,
            [FromBody] UpdateLessonProgressRequest request)
        {
            var result = await _lessonProgressService.UpdateProgressAsync(enrollmentId, request);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách LessonProgress của 1 Enrollment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProgressByEnrollment(int enrollmentId)
        {
            var result = await _lessonProgressService.GetProgressByEnrollmentAsync(enrollmentId);
            return Ok(result);
        }
    }
}

