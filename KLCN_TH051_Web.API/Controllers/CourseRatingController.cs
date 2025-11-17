using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/courses/{courseId}/ratings")]
    [ApiController]
    public class CourseRatingController : ControllerBase
    {
        private readonly ICourseRatingService _courseRatingService;

        public CourseRatingController(ICourseRatingService courseRatingService)
        {
            _courseRatingService = courseRatingService;
        }

        /// <summary>
        /// Tạo đánh giá cho khóa học
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRating(int courseId, [FromBody] CreateCourseRatingRequest request)
        {
            // đảm bảo request.CourseId đúng với route
            request.CourseId = courseId;

            var result = await _courseRatingService.CreateRatingAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật đánh giá
        /// </summary>
        [HttpPut("{ratingId}")]
        public async Task<IActionResult> UpdateRating(int courseId, int ratingId, [FromBody] UpdateCourseRatingRequest request)
        {
            var result = await _courseRatingService.UpdateRatingAsync(ratingId, request);
            return Ok(result);
        }

        /// <summary>
        /// Xóa đánh giá
        /// </summary>
        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int courseId, int ratingId)
        {
            await _courseRatingService.DeleteRatingAsync(ratingId);
            return NoContent();
        }

        /// <summary>
        /// Lấy tất cả đánh giá của khóa học
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRatingsByCourse(int courseId)
        {
            var result = await _courseRatingService.GetRatingsByCourseAsync(courseId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy chi tiết một đánh giá
        /// </summary>
        [HttpGet("{ratingId}")]
        public async Task<IActionResult> GetRatingById(int courseId, int ratingId)
        {
            var result = await _courseRatingService.GetRatingByIdAsync(ratingId);
            return Ok(result);
        }
    }
}
