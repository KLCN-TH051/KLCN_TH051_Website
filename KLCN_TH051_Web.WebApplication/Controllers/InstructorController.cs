using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class InstructorController : Controller
    {
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        //Dùng cái này khi có backend
        //public IActionResult Courses(int? id)
        //{
        //    return id == null
        //        ? View()
        //        : View("Detail");
        //}

        public IActionResult Orders()
        {
            return View();
        }
    }
}
