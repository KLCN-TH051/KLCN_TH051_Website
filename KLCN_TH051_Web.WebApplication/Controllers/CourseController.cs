using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
