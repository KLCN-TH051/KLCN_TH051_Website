using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            // Chỉ cần trả về View hiển thị nút đăng nhập
            return View();
        }

        public IActionResult Register()
        {
            return View();
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
