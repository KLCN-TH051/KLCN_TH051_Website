using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Route("Instructor/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }
    }
}
