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

            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }
        public IActionResult Role()
        {

            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }

        public IActionResult TeacherAssignment()
        {

            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }



        public IActionResult Index()
        {

            ViewData["ApiUrl"] = _configuration["ApiUrl"];
            return View();
        }
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //public IActionResult Edit(int id)
        //{
        //    return View();
        //}

        //public IActionResult Details(int id)
        //{
        //    return View();
        //}

        //public IActionResult Delete(int id)
        //{
        //    return View();
        //}
    }
}
