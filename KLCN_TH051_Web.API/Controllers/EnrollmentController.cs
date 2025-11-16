using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _service;

        public EnrollmentController(IEnrollmentService service)
        {
            _service = service;
        }

        // POST: api/enrollments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
        {
            var result = await _service.CreateEnrollmentAsync(request);
            return Ok(result);
        }

        // GET: api/enrollments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetEnrollmentByIdAsync(id);
            return Ok(result);
        }

        // GET: api/enrollments/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(int studentId)
        {
            var result = await _service.GetEnrollmentsByStudentAsync(studentId);
            return Ok(result);
        }

        // PUT: api/enrollments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentRequest request)
        {
            var result = await _service.UpdateEnrollmentAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/enrollments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteEnrollmentAsync(id);
            return NoContent();
        }
    }
}
