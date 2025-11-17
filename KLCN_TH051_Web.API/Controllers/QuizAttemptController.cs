using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/quiz-attempts")]
    [ApiController]
    public class QuizAttemptController : ControllerBase
    {
        private readonly IQuizAttemptService _service;

        public QuizAttemptController(IQuizAttemptService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuizAttemptRequest request)
        {
            var result = await _service.CreateQuizAttemptAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateQuizAttemptRequest request)
        {
            var result = await _service.UpdateQuizAttemptAsync(id, request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetQuizAttemptByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("quiz/{quizId}/student/{studentId}")]
        public async Task<IActionResult> GetAttempts(int quizId, int studentId)
        {
            var result = await _service.GetAttemptsByQuizAsync(quizId, studentId);
            return Ok(result);
        }
    }
}
