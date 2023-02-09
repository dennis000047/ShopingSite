using Microsoft.AspNetCore.Mvc;

namespace SIEG_ADMIN.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
