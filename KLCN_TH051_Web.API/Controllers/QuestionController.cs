using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // ---------------------------------------------------------
        // Tạo question mới
        // POST: api/question
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            string creatorId = User.Identity?.Name ?? "system";

            var result = await _questionService.CreateQuestionAsync(request, creatorId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy danh sách question theo quiz
        // GET: api/question/quiz/5
        // ---------------------------------------------------------
        [HttpGet("quiz/{quizId}")]
        public async Task<IActionResult> GetQuestionsByQuiz(int quizId)
        {
            var result = await _questionService.GetQuestionsByQuizAsync(quizId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Lấy chi tiết question
        // GET: api/question/10
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById(int id)
        {
            var result = await _questionService.GetQuestionByIdAsync(id);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Cập nhật question
        // PUT: api/question/10
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] UpdateQuestionRequest request)
        {
            string updaterId = User.Identity?.Name ?? "system";

            var result = await _questionService.UpdateQuestionAsync(id, request, updaterId);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // Xóa question (soft delete)
        // DELETE: api/question/10
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            string deleterId = User.Identity?.Name ?? "system";

            await _questionService.DeleteQuestionAsync(id, deleterId);
            return NoContent();
        }

        // ---------------------------------------------------------
        // Reorder question trong quiz
        // POST: api/question/reorder/5
        // BODY: [3,1,2]
        // ---------------------------------------------------------
        [HttpPost("reorder/{quizId}")]
        public async Task<IActionResult> ReorderQuestions(int quizId, [FromBody] List<int> questionIdsInNewOrder)
        {
            await _questionService.ReorderQuestionsAsync(quizId, questionIdsInNewOrder);
            return Ok(new { message = "Question order updated successfully" });
        }
    }
}
