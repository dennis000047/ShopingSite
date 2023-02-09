using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class news_listController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
