using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/chapters/{chapterId}/lessons")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // --------------------------------------------------------------------
        // CREATE LESSON
        // POST api/chapters/{chapterId}/lessons
        // --------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateLesson(int chapterId, [FromBody] CreateLessonRequest request)
        {
            string creatorId = User.Identity?.Name ?? "system";
            var result = await _lessonService.CreateLessonAsync(chapterId, request, creatorId);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET LESSON LIST
        // GET api/chapters/{chapterId}/lessons
        // --------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetLessonsByChapter(int chapterId)
        {
            var result = await _lessonService.GetLessonsByChapterAsync(chapterId);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET LESSON DETAIL
        // GET api/chapters/{chapterId}/lessons/{id}
        // --------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int chapterId, int id)
        {
            var result = await _lessonService.GetLessonByIdAsync(id);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // UPDATE LESSON
        // PUT api/chapters/{chapterId}/lessons/{id}
        // --------------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int chapterId, int id, [FromBody] UpdateLessonRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";
            var result = await _lessonService.UpdateLessonAsync(id, request, updaterId);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // DELETE LESSON (soft delete)
        // DELETE api/chapters/{chapterId}/lessons/{id}
        // --------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int chapterId, int id)
        {
            string deleterId = User.Identity?.Name ?? "system";
            await _lessonService.DeleteLessonAsync(id, deleterId);
            return NoContent();
        }

        // --------------------------------------------------------------------
        // REORDER LESSONS
        // POST api/chapters/{chapterId}/lessons/reorder
        // BODY: [3,1,2]
        // --------------------------------------------------------------------
        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderLessons(int chapterId, [FromBody] List<int> lessonIdsInNewOrder)
        {
            await _lessonService.ReorderLessonsAsync(chapterId, lessonIdsInNewOrder);
            return Ok(new { message = "Lesson order updated successfully" });
        }
    }
}
