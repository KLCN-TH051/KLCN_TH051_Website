using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        // GET: api/Subjects
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<SubjectResponse>>> GetAll()
        {
            var subjects = await _subjectService.GetAllAsync();
            return Ok(subjects);
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<SubjectResponse>> GetById(int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null) return NotFound();
            return Ok(subject);
        }

        // POST: api/Subjects
        [HttpPost]
        [Authorize(Roles = "Admin")] // Chỉ admin mới được tạo
        public async Task<ActionResult<SubjectResponse>> Create([FromBody] CreateSubjectRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _subjectService.CreateAsync(request, userId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/Subjects/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ admin mới được cập nhật
        public async Task<ActionResult<SubjectResponse>> Update(int id, [FromBody] UpdateSubjectRequest request)
        {
            var result = await _subjectService.UpdateAsync(id, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Chỉ admin mới được xóa
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _subjectService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
