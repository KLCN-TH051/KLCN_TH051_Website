using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Statistics()
        {
            ViewData["ActiveMenu"] = "stats";
            return View();
        }

        public IActionResult Courses()
        {
            ViewData["ActiveMenu"] = "courses";
            return View();
        }

        public IActionResult Orders()
        {
            ViewData["ActiveMenu"] = "orders";
            return View();
        }
    }
}
