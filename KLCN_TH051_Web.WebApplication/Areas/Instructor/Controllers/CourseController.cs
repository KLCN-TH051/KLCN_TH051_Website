using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class CourseController : Controller
    {
        private readonly IConfiguration _configuration;

        public CourseController(IConfiguration configuration)
        {
            _configuration = configuration;
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

    }

}
