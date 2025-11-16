using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoContentController : ControllerBase
    {
        private readonly IVideoContentService _videoContentService;

        public VideoContentController(IVideoContentService videoContentService)
        {
            _videoContentService = videoContentService;
        }

        // ---------------------------------------------------------
        // Tạo video mới
        // POST: api/videoContent
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateVideoContent([FromBody] CreateVideoContentRequest request)
        {
            string creatorId = User.Identity?.Name ?? "system";
            var result = await _videoContentService.CreateVideoContentAsync(request, creatorId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy danh sách video theo lesson
        // GET: api/videoContent/lesson/5
        // ---------------------------------------------------------
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetVideoContentsByLesson(int lessonId)
        {
            var result = await _videoContentService.GetVideoContentsByLessonAsync(lessonId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy chi tiết video
        // GET: api/videoContent/10
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideoContentById(int id)
        {
            var result = await _videoContentService.GetVideoContentByIdAsync(id);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Cập nhật video
        // PUT: api/videoContent/10
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVideoContent(int id, [FromBody] UpdateVideoContentRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";
            var result = await _videoContentService.UpdateVideoContentAsync(id, request, updaterId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Xóa video (soft delete)
        // DELETE: api/videoContent/10
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoContent(int id)
        {
            string deleterId = User.Identity?.Name ?? "system";
            await _videoContentService.DeleteVideoContentAsync(id, deleterId);
            return NoContent();
        }
    }
}
