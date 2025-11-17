using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    public class Course : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
