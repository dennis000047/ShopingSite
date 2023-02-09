using Microsoft.AspNetCore.Mvc;

namespace SIEG_ADMIN.Controllers
{
    public class newsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult add()
        {
            return View();
        }
        public IActionResult edit()
        {
            return View();
        }
        public IActionResult category()
        {
            return View();
        }
        public IActionResult category_add()
        {
            return View();
        }
        public IActionResult category_edit()
        {
            return View();
        }
    }
}
