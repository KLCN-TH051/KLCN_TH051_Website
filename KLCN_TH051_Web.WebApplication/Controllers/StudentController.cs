using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Courses()
        {
            return View();
        }
    }
}
