using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Trang cá nhân";
            return View();
        }
    }
}
