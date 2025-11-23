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

            // Giới hạn định dạng
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ cho phép file hình (.jpg, .png, .gif)");

            // Giới hạn kích thước (5MB)
            long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                return BadRequest("File quá lớn, tối đa 5MB");

            // Đường dẫn tới wwwroot/images/courses
            var folderPath = Path.Combine(_env.WebRootPath, "images/courses");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(folderPath, fileName);

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về URL để FE dùng luôn
            var fileUrl = $"/images/courses/{fileName}";

            return Ok(new
            {
                fileName,
                fileUrl
            });
        }

        [HttpPost("ContentImage")]
        public async Task<IActionResult> UploadContentImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File rỗng");

            // Giới hạn định dạng
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ cho phép file hình (.jpg, .png, .gif)");

            // Giới hạn kích thước (5MB)
            long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                return BadRequest("File quá lớn, tối đa 5MB");

            // Đường dẫn tới wwwroot/images/contents
            var folderPath = Path.Combine(_env.WebRootPath, "images/contents");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(folderPath, fileName);

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/images/contents/{fileName}";

            return Ok(new
            {
                fileName,
                fileUrl
            });
        }


        [HttpPost("Avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File rỗng");

            // Giới hạn định dạng
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                return BadRequest("Chỉ cho phép file hình (.jpg, .png, .gif)");

            // Giới hạn kích thước (5MB)
            long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize)
                return BadRequest("File quá lớn, tối đa 5MB");

            // Đường dẫn tới wwwroot/images/avatars
            var folderPath = Path.Combine(_env.WebRootPath, "images/avatars");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(folderPath, fileName);

            // Lưu file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về URL để FE dùng luôn
            var fileUrl = $"/images/avatars/{fileName}";

            return Ok(new
            {
                fileName,
                fileUrl
            });
        }

        [HttpGet("courseimage/{fileName}")]
        public IActionResult GetCourseImage(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images/courses", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/" + Path.GetExtension(fileName).TrimStart('.');
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }

        [HttpGet("ContentImage/{fileName}")]
        public IActionResult GetContentImage(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images/contents", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/" + Path.GetExtension(fileName).TrimStart('.');
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }


        [HttpGet("Avatar/{fileName}")]
        public IActionResult GetAvatar(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "images/avatars", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/" + Path.GetExtension(fileName).TrimStart('.');
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }

    }
}
