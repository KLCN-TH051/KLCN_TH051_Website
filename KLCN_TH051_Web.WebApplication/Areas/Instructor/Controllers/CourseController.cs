using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Route("Instructor/[controller]/[action]")]
    public class CourseController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public CourseController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        public IActionResult Index()
        {

            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            ViewData["Title"] = "Khoá học giảng dạy";
            return View();
        }
        [HttpGet("/Instructor/Course/Detail/{id}")]
        public IActionResult Details(int id)
        {
            ViewData["CourseId"] = id;
            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }

        // Controller: CourseController.cs hoặc LessonController.cs
        public IActionResult LessonDetail(int lessonId, int chapterId, int? courseId = null)
        {
            // Dùng để hiển thị breadcrumb, menu, hoặc load danh sách bài học bên cạnh
            ViewData["CourseId"] = courseId ?? 0;
            ViewData["ChapterId"] = chapterId;
            ViewData["LessonId"] = lessonId;

            // Nếu bạn có cấu hình API riêng
            // ViewData["ApiUrl"] = _configuration["ApiUrl"];

            return View();
        }

    }

}
