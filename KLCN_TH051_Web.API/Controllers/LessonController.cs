using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        // ---------------------------------------------------------
        // Tạo lesson mới
        // POST: api/lesson/{chapterId}
        // ---------------------------------------------------------
        [HttpPost("{chapterId}")]
        public async Task<IActionResult> CreateLesson(int chapterId, [FromBody] CreateLessonRequest request)
        {
            // Ở đây bạn lấy Id từ token nếu cần
            string creatorId = User.Identity?.Name ?? "system";

            var result = await _lessonService.CreateLessonAsync(chapterId, request, creatorId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy danh sách bài học trong chapter
        // GET: api/lesson/chapter/5
        // ---------------------------------------------------------
        [HttpGet("chapter/{chapterId}")]
        public async Task<IActionResult> GetLessonsByChapter(int chapterId)
        {
            var result = await _lessonService.GetLessonsByChapterAsync(chapterId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy chi tiết bài học
        // GET: api/lesson/10
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var result = await _lessonService.GetLessonByIdAsync(id);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Cập nhật bài học
        // PUT: api/lesson/10
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";

            var result = await _lessonService.UpdateLessonAsync(id, request, updaterId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Xóa bài học (soft delete)
        // DELETE: api/lesson/10
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            string deleterId = User.Identity?.Name ?? "system";

            await _lessonService.DeleteLessonAsync(id, deleterId);
            return NoContent();
        }

        // ---------------------------------------------------------
        // Reorder bài học trong chapter
        // POST: api/lesson/reorder/5
        // BODY: [3,1,2]
        // ---------------------------------------------------------
        [HttpPost("reorder/{chapterId}")]
        public async Task<IActionResult> ReorderLessons(int chapterId, [FromBody] List<int> lessonIdsInNewOrder)
        {
            await _lessonService.ReorderLessonsAsync(chapterId, lessonIdsInNewOrder);
            return Ok(new { message = "Lesson order updated successfully" });
        }

    }
}
