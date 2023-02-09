using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class orderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
