using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        // ==============================================
        // GET: /api/answer/{id}
        // ==============================================
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAnswerById(int id)
        {
            var result = await _answerService.GetAnswerByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Answer not found" });

            return Ok(result);
        }

        // ==============================================
        // GET: /api/answer/question/{questionId}
        // ==============================================
        [HttpGet("question/{questionId:int}")]
        public async Task<IActionResult> GetAnswersByQuestion(int questionId)
        {
            var result = await _answerService.GetAnswersByQuestionAsync(questionId);
            return Ok(result);
        }

        // ==============================================
        // POST: /api/answer
        // ==============================================
        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _answerService.CreateAnswerAsync(request);

            return CreatedAtAction(nameof(GetAnswerById), new { id = result.Id }, result);
        }

        // ==============================================
        // PUT: /api/answer/{id}
        // ==============================================
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnswer(int id, [FromBody] UpdateAnswerRequest request)
        {
            try
            {
                var result = await _answerService.UpdateAnswerAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ==============================================
        // DELETE: /api/answer/{id}
        // ==============================================
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var success = await _answerService.DeleteAnswerAsync(id);
            if (!success)
                return NotFound(new { message = "Answer not found" });

            return Ok(new { message = "Answer deleted successfully" });
        }
    }
}
