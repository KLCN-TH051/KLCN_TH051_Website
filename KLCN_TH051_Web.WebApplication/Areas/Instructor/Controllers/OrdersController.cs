using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
