using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult User()
        {
            return View();
        }

        public IActionResult Role()
        {
            return View();
        }

        public IActionResult TeacherAssignment()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
