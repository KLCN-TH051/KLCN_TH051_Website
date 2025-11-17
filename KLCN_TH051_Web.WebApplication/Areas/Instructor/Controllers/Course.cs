using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class Course : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Khoá học giảng dạy";
            return View();
        }
    }
}
