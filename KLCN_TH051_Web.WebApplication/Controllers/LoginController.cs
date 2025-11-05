using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Google()
        {
            // Chỉ cần trả về View hiển thị nút đăng nhập
            return View();
        }
    }
}
