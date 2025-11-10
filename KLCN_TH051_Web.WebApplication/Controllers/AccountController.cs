using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration qua constructor
        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Chỉ cần trả về View hiển thị nút đăng nhập
            return View();
        }

        public IActionResult Register()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl"]; // Lấy từ appsettings.json
            return View();
        }

        [HttpGet("confirm-email")]
        public IActionResult ConfirmEmail()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View(); // MVC sẽ tìm Views/Account/ConfirmEmail.cshtml
        }

        public IActionResult ForgotPass()
        {
            return View();
        }

        [HttpGet("Account/ResetPasswordForm")]
        public IActionResult ResetPasswordForm()
        {
            return View("ResetPassword");
        }

        public IActionResult ResetSuccess()
        {
            return View();
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        public IActionResult VerifySuccess()
        {
            return View();
        }
    }
}
