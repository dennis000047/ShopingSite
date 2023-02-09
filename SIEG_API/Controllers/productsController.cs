using Microsoft.AspNetCore.Mvc;

namespace SIEG_API.Controllers
{
    public class productsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
