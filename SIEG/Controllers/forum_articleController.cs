using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class forum_articleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
