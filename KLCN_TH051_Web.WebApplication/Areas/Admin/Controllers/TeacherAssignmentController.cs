using Microsoft.AspNetCore.Mvc;

namespace KLCN_TH051_Web.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherAssignmentController : Controller
    {
        private readonly IConfiguration _configuration;

        public TeacherAssignmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }
    }
}
