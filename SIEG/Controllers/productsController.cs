using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class productsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
