using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class forum_listController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
