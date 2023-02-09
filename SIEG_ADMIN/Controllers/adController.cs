using Microsoft.AspNetCore.Mvc;

namespace SIEG_ADMIN.Controllers
{
    public class adController : Controller
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
    }
}
