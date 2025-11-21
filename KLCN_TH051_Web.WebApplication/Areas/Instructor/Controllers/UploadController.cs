using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCourseImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File rỗng");

            // đúng chuẩn: đường dẫn thật của wwwroot
            var folder = Path.Combine(_env.WebRootPath, "images/courses");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }
    }
}
