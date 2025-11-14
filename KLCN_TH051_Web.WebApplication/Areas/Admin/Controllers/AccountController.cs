using Microsoft.AspNetCore.Mvc;

namespace YourProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý tài khoản";
            ViewData["Active"] = "Account";
            return View();
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Thêm tài khoản";
            ViewData["Active"] = "Account";
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Chỉnh sửa tài khoản";
            ViewData["Active"] = "Account";
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewData["Title"] = "Chi tiết tài khoản";
            ViewData["Active"] = "Account";
            return View();
        }

        public IActionResult Delete(int id)
        {
            ViewData["Title"] = "Xác nhận xóa tài khoản";
            ViewData["Active"] = "Account";
            return View();
        }
    }
}
