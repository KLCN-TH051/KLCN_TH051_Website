using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Enums;
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

        // POST: api/chapters/{chapterId}/lessons
        [HttpPost]
        public async Task<ActionResult<LessonResponse>> CreateLesson(int chapterId, [FromBody] CreateLessonRequest request)
        {
            try
            {
                string createdBy = User.Identity?.Name ?? "system";
                var lesson = await _lessonService.CreateLessonAsync(request, chapterId, createdBy);
                return CreatedAtAction(nameof(GetLessonById), new { chapterId = chapterId, lessonId = lesson.Id }, lesson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/chapters/{chapterId}/lessons/{lessonId}/content
        [HttpPut("{lessonId}/content")]
        public async Task<ActionResult<LessonResponse>> UpdateLessonContent(int chapterId, int lessonId, [FromBody] UpdateLessonContentRequest request)
        {
            try
            {
                string updatedBy = User.Identity?.Name ?? "system";
                var lesson = await _lessonService.UpdateLessonContentAsync(lessonId, request, updatedBy);
                return Ok(lesson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/chapters/{chapterId}/lessons/{lessonId}/video
        [HttpPut("{lessonId}/video")]
        public async Task<ActionResult<LessonResponse>> UpdateLessonVideo(int chapterId, int lessonId, [FromBody] UpdateLessonVideoRequest request)
        {
            try
            {
                string updatedBy = User.Identity?.Name ?? "system";
                var lesson = await _lessonService.UpdateLessonVideoAsync(lessonId, request, updatedBy);
                return Ok(lesson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/chapters/{chapterId}/lessons/{lessonId}
        [HttpDelete("{lessonId}")]
        public async Task<ActionResult> DeleteLesson(int chapterId, int lessonId)
        {
            string deletedBy = User.Identity?.Name ?? "system";
            var result = await _lessonService.DeleteLessonAsync(lessonId, deletedBy);
            if (!result) return NotFound();
            return NoContent();
        }

        // GET: api/chapters/{chapterId}/lessons/{lessonId}
        [HttpGet("{lessonId}")]
        public async Task<ActionResult<LessonResponse>> GetLessonById(int chapterId, int lessonId)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(lessonId);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        // GET: api/chapters/{chapterId}/lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonResponse>>> GetLessonsByChapter(int chapterId)
        {
            var lessons = await _lessonService.GetLessonsByChapterAsync(chapterId);
            return Ok(lessons);
        }

        // GET 
        [HttpGet("~/api/lesson-types")]
        public IActionResult GetLessonTypes()
        {
            var types = Enum.GetValues(typeof(LessonType))
                .Cast<LessonType>()
                .Select(t => new
                {
                    value = (int)t,
                    label = t switch
                    {
                        LessonType.Content => "Bài đọc",
                        LessonType.Video => "Video",
                        LessonType.Quiz => "Quiz (Bài kiểm tra)",
                        _ => t.ToString()
                    }
                })
                .OrderBy(t => t.value);

            return Ok(types);
        }
    }
}
