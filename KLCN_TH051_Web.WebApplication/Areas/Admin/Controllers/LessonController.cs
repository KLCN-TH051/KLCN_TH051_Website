using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Banners()
        {
            return View();
        }
    }
}
