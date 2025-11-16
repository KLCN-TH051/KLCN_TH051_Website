using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [ApiController]
    [Route("api/courses/{courseId}/chapters")]
    public class ChaptersController : ControllerBase
    {
        private readonly IChapterService _chapterService;

        public ChaptersController(IChapterService chapterService)
        {
            _chapterService = chapterService;
        }

        // -----------------------------
        // Tạo chapter
        // -----------------------------
        [HttpPost]
        public async Task<IActionResult> CreateChapter(int courseId, [FromBody] CreateChapterRequest request)
        {
            try
            {
                var chapter = await _chapterService.CreateChapterAsync(courseId, request);
                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -----------------------------
        // Lấy danh sách chapter theo khóa học
        // -----------------------------
        [HttpGet]
        public async Task<IActionResult> GetChapters(int courseId)
        {
            try
            {
                var chapters = await _chapterService.GetChaptersByCourseAsync(courseId);
                return Ok(chapters);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -----------------------------
        // Lấy chi tiết chapter
        // -----------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChapter(int courseId, int id)
        {
            try
            {
                var chapter = await _chapterService.GetChapterByIdAsync(id);
                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // -----------------------------
        // Cập nhật chapter (chỉ Name)
        // -----------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChapter(int courseId, int id, [FromBody] UpdateChapterRequest request)
        {
            try
            {
                var chapter = await _chapterService.UpdateChapterAsync(id, request);
                return Ok(chapter);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -----------------------------
        // Xóa chapter
        // -----------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChapter(int courseId, int id)
        {
            try
            {
                await _chapterService.DeleteChapterAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // -----------------------------
        // Reorder chapter (tuỳ chọn)
        // client gửi danh sách Id chapter theo thứ tự mới
        // -----------------------------
        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderChapters(int courseId, [FromBody] List<int> chapterIdsInNewOrder)
        {
            try
            {
                await _chapterService.ReorderChaptersAsync(courseId, chapterIdsInNewOrder);
                return Ok(new { message = "Chapters reordered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        }
}
