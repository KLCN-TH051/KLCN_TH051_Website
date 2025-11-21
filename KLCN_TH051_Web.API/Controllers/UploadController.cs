using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("CourseImage")]
        public async Task<IActionResult> UploadCourseImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File rỗng");

            // Đường dẫn tới wwwroot/images/courses
            var folderPath = Path.Combine(_env.WebRootPath, "images/courses");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về tên file cho frontend lưu vào cơ sở dữ liệu
            return Ok(new { fileName });
        }
    }
}
