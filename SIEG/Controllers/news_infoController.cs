using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class news_infoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
