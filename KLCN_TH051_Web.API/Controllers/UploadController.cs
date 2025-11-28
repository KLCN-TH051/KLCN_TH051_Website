using KLCN_TH051_Website.Common.DTO.Requests;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace KLCN_TH051_Web.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;

        public UploadController(IWebHostEnvironment env, IQuestionService questionService, IAnswerService answerService)
        {
            _env = env;
            _questionService = questionService;
            _answerService = answerService;
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
        // Upload Image endpoints
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

        [HttpPost("BannerImage")]
        public async Task<IActionResult> UploadBannerImage([FromForm] IFormFile file)
        {
            try
            {
                var result = await SaveFileAsync(file, "images/banners", new[] { ".jpg", ".jpeg", ".png", ".gif" }, 5 * 1024 * 1024);
                return Ok(new { result.fileName, result.fileUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -----------------------
        // Upload Excel câu hỏi + đáp án
        // -----------------------
        [HttpPost("QuestionsExcel")]
        public async Task<IActionResult> UploadQuestionsExcel([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Chưa chọn file");

            var (fileName, fileUrl) = await SaveFileAsync(file, "uploads/excel", new[] { ".xlsx", ".xls" }, 10 * 1024 * 1024);

            var questions = new List<CreateQuestionRequest>();
            var answers = new List<CreateAnswerRequest>();

            using (var stream = file.OpenReadStream())
            {
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    // -------- Câu hỏi ----------
                    var questionText = worksheet.Cells[row, 1].Text;
                    var points = decimal.TryParse(worksheet.Cells[row, 2].Text, out var p) ? p : 0;
                    var quizId = int.TryParse(worksheet.Cells[row, 3].Text, out var qId) ? qId : 0;

                    var questionReq = new CreateQuestionRequest
                    {
                        QuestionText = questionText,
                        Points = points,
                        QuizId = quizId
                    };

                    // Tạo câu hỏi trước để lấy QuestionId
                    var createdQuestion = await _questionService.CreateQuestionAsync(questionReq, User.Identity?.Name ?? "System");

                    // -------- Đáp án ---------
                    for (int col = 4; col <= colCount; col += 2)
                    {
                        var answerText = worksheet.Cells[row, col].Text;
                        if (string.IsNullOrWhiteSpace(answerText)) continue;

                        var isCorrect = bool.TryParse(worksheet.Cells[row, col + 1].Text, out var c) && c;

                        var answerReq = new CreateAnswerRequest
                        {
                            QuestionId = createdQuestion.Id,
                            AnswerText = answerText,
                            IsCorrect = isCorrect
                        };
                        answers.Add(answerReq);
                    }
                }
            }

            // Tạo tất cả đáp án
            if (answers.Any())
                await _answerService.CreateManyAnswersAsync(answers);

            return Ok(new { fileName, fileUrl, questionCount = questions.Count, answerCount = answers.Count });
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
                "banner" => "images/banners",
                _ => null
            };

            if (folder == null) return BadRequest("Type không hợp lệ");

            var filePath = Path.Combine(_env.WebRootPath, folder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            string ext = Path.GetExtension(fileName).ToLower();
            string contentType = ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".xlsx" or ".xls" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }
    }
}