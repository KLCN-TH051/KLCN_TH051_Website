using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }
    }
}
