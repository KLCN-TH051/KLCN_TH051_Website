using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // ---------------------------------------
        // Tạo quiz mới
        // POST: api/quiz
        // ---------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
        {
            string creatorId = User.Identity?.Name ?? "system";
            var result = await _quizService.CreateQuizAsync(request, creatorId);
            return Ok(result);
        }

        // ---------------------------------------
        // Lấy danh sách quiz theo lesson
        // GET: api/quiz/lesson/5
        // ---------------------------------------
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetQuizzesByLesson(int lessonId)
        {
            var result = await _quizService.GetQuizzesByLessonAsync(lessonId);
            return Ok(result);
        }

        // ---------------------------------------
        // Lấy chi tiết quiz
        // GET: api/quiz/10
        // ---------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuizById(int id)
        {
            var result = await _quizService.GetQuizByIdAsync(id);
            return Ok(result);
        }

        // ---------------------------------------
        // Cập nhật quiz
        // PUT: api/quiz/10
        // ---------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";
            var result = await _quizService.UpdateQuizAsync(id, request, updaterId);
            return Ok(result);
        }

        // ---------------------------------------
        // Xóa quiz (soft delete)
        // DELETE: api/quiz/10
        // ---------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            string deleterId = User.Identity?.Name ?? "system";
            await _quizService.DeleteQuizAsync(id, deleterId);
            return NoContent();
        }
    }
}
