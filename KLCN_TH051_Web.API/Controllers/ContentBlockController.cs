using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentBlockController : ControllerBase
    {
        private readonly IContentBlockService _contentBlockService;

        public ContentBlockController(IContentBlockService contentBlockService)
        {
            _contentBlockService = contentBlockService;
        }

        // ---------------------------------------------------------
        // Tạo content block mới
        // POST: api/contentblock
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateContentBlock([FromBody] CreateContentBlockRequest request)
        {
            string creatorId = User.Identity?.Name ?? "system";
            var result = await _contentBlockService.CreateContentBlockAsync(request, creatorId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy danh sách content block theo lesson
        // GET: api/contentblock/lesson/5
        // ---------------------------------------------------------
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetContentBlocksByLesson(int lessonId)
        {
            var result = await _contentBlockService.GetContentBlocksByLessonAsync(lessonId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy chi tiết content block
        // GET: api/contentblock/10
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentBlockById(int id)
        {
            var result = await _contentBlockService.GetContentBlockByIdAsync(id);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Cập nhật content block
        // PUT: api/contentblock/10
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContentBlock(int id, [FromBody] UpdateContentBlockRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";
            var result = await _contentBlockService.UpdateContentBlockAsync(id, request, updaterId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Xóa content block (soft delete)
        // DELETE: api/contentblock/10
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContentBlock(int id)
        {
            string deleterId = User.Identity?.Name ?? "system";
            await _contentBlockService.DeleteContentBlockAsync(id, deleterId);
            return NoContent();
        }

        // ---------------------------------------------------------
        // Reorder content block trong lesson
        // POST: api/contentblock/reorder/5
        // BODY: [3,1,2]
        // ---------------------------------------------------------
        [HttpPost("reorder/{lessonId}")]
        public async Task<IActionResult> ReorderContentBlocks(int lessonId, [FromBody] List<int> contentBlockIdsInNewOrder)
        {
            await _contentBlockService.ReorderContentBlocksAsync(lessonId, contentBlockIdsInNewOrder);
            return Ok(new { message = "Content block order updated successfully" });
        }
    }
}
