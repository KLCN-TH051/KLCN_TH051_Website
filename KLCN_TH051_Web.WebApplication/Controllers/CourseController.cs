using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
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
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        public IActionResult Lessons()
        {
            return View();
        }
    }
}
