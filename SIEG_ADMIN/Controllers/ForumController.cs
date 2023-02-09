using Microsoft.AspNetCore.Mvc;

namespace SIEG_ADMIN.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
