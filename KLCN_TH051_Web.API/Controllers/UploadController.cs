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

        // -----------------------
        // Private helper
        // -----------------------
        private async Task<(string fileName, string fileUrl)> SaveFileAsync(IFormFile file, string folder, string[] allowedExtensions, long maxSize)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File rỗng");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                throw new Exception($"Chỉ cho phép các định dạng: {string.Join(", ", allowedExtensions)}");

            if (file.Length > maxSize)
                throw new Exception($"File quá lớn, tối đa {maxSize / (1024 * 1024)}MB");

            var folderPath = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/{folder}/{fileName}";
            return (fileName, fileUrl);
        }

        // -----------------------
        // Upload Course Image
        // -----------------------
        [HttpPost("CourseImage")]
        public async Task<IActionResult> UploadCourseImage([FromForm] IFormFile file)
        {
            try
            {
                var result = await SaveFileAsync(file, "images/courses", new[] { ".jpg", ".jpeg", ".png", ".gif" }, 5 * 1024 * 1024);
                return Ok(new { result.fileName, result.fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------
        // Upload Lesson/Content Image
        // -----------------------
        [HttpPost("ContentImage")]
        public async Task<IActionResult> UploadContentImage([FromForm] IFormFile file)
        {
            try
            {
                var result = await SaveFileAsync(file, "images/contents", new[] { ".jpg", ".jpeg", ".png", ".gif" }, 5 * 1024 * 1024);
                return Ok(new { result.fileName, result.fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------
        // Upload Avatar
        // -----------------------
        [HttpPost("Avatar")]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
        {
            try
            {
                var result = await SaveFileAsync(file, "images/avatars", new[] { ".jpg", ".jpeg", ".png", ".gif" }, 5 * 1024 * 1024);
                return Ok(new { result.fileName, result.fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------
        // Upload Lesson Video
        // -----------------------
        [HttpPost("LessonVideo")]
        public async Task<IActionResult> UploadLessonVideo([FromForm] IFormFile file)
        {
            try
            {
                var result = await SaveFileAsync(file, "videos/lessons", new[] { ".mp4", ".mov", ".avi", ".mkv" }, 500 * 1024 * 1024); // 500MB max
                return Ok(new { result.fileName, result.fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------
        // Get file by type
        // -----------------------
        [HttpGet("{type}/{fileName}")]
        public IActionResult GetFile(string type, string fileName)
        {
            string? folder = type.ToLower() switch
            {
                "course" => "images/courses",
                "content" => "images/contents",
                "avatar" => "images/avatars",
                "video" => "videos/lessons",
                _ => null
            };


            if (folder == null) return BadRequest("Type không hợp lệ");

            var filePath = Path.Combine(_env.WebRootPath, folder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            // MIME type cơ bản
            string ext = Path.GetExtension(fileName).ToLower();
            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".mp4" => "video/mp4",
                ".mov" => "video/quicktime",
                ".avi" => "video/x-msvideo",
                ".mkv" => "video/x-matroska",
                _ => "application/octet-stream"
            };

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }
    }
}