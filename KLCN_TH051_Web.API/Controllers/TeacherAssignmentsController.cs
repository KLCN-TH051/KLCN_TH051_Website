using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.DTO.Responses;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly ITeacherAssignmentService _service;

        public TeacherAssignmentsController(ITeacherAssignmentService service)
        {
            _service = service;
        }

        // -----------------------------------
        // GET: api/TeacherAssignments
        // Lấy tất cả phân công
        // -----------------------------------
        [HttpGet]
        [Authorize(Policy = "TeacherAssignment.ViewAll")]
        public async Task<ActionResult<List<TeacherAssignmentResponse>>> GetAll()
        {
            var assignments = await _service.GetAllAsync();
            return Ok(assignments);
        }

        // -----------------------------------
        // GET: api/TeacherAssignments/teacher/{teacherId}
        // Lấy danh sách môn học theo giáo viên
        // -----------------------------------
        [HttpGet("teacher/{teacherId}")]
        [Authorize(Policy = "TeacherAssignment.ViewByTeacher")]
        public async Task<ActionResult<List<TeacherSubjectResponse>>> GetSubjectsByTeacher(int teacherId)
        {
            var subjects = await _service.GetSubjectsByTeacherAsync(teacherId);
            return Ok(subjects);
        }

        // -----------------------------------
        // POST: api/TeacherAssignments
        // Tạo phân công mới
        // -----------------------------------
        [HttpPost]
        [Authorize(Policy = "TeacherAssignment.Create")]
        public async Task<ActionResult<TeacherAssignmentResponse>> Create([FromBody] CreateTeacherAssignmentRequest request)
        {
            var assignment = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = assignment.Id }, assignment);
        }

        // -----------------------------------
        // PUT: api/TeacherAssignments/{id}
        // Update phân công
        // -----------------------------------
        [HttpPut("{id}")]
        [Authorize(Policy = "TeacherAssignment.Edit")]
        public async Task<ActionResult<TeacherAssignmentResponse>> Update(int id, [FromBody] UpdateTeacherAssignmentRequest request)
        {
            var assignment = await _service.UpdateAsync(id, request);
            return Ok(assignment);
        }

        // -----------------------------------
        // DELETE: api/TeacherAssignments/{id}
        // Xóa phân công
        // -----------------------------------
        [HttpDelete("{id}")]
        [Authorize(Policy = "TeacherAssignment.Delete")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "TeacherAssignment.View")]
        public async Task<ActionResult<TeacherAssignmentResponse>> GetById(int id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
