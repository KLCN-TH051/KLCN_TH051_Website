using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
