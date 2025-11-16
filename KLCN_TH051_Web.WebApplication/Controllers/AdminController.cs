using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }

        public IActionResult Banners()
        {
            return View();
        }
    }
}
