using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/lessons/{lessonId}/comments")]
    [ApiController]
    public class LessonCommentController : ControllerBase
    {
        private readonly ILessonCommentService _commentService;

        public LessonCommentController(ILessonCommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Tạo comment mới cho lesson (hoặc reply)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateComment(int lessonId, [FromBody] CreateLessonCommentRequest request)
        {
            if (request.LessonId != lessonId)
                return BadRequest("LessonId mismatch");

            var result = await _commentService.CreateCommentAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Cập nhật comment
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateLessonCommentRequest request)
        {
            var result = await _commentService.UpdateCommentAsync(id, request);
            return Ok(result);
        }

        /// <summary>
        /// Xóa comment (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Lấy danh sách comment của lesson (chỉ comment cha + reply)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCommentsByLesson(int lessonId)
        {
            var result = await _commentService.GetCommentsByLessonAsync(lessonId);
            return Ok(result);
        }

        /// <summary>
        /// Lấy 1 comment theo id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var result = await _commentService.GetCommentByIdAsync(id);
            return Ok(result);
        }
    }
}
