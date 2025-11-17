using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class RankingController : Controller
    {
        // Trang chọn khóa học để xem xếp hạng
        public IActionResult Index()
        {
            ViewData["Title"] = "Xếp hạng học viên";
            return View();
        }

        // Trang hiển thị bảng xếp hạng của 1 khóa học cụ thể
        public IActionResult CourseRanking(int courseId)
        {
            ViewData["Title"] = "Bảng xếp hạng học viên";
            ViewBag.CourseId = courseId;
            return View();
        }
    }
}
