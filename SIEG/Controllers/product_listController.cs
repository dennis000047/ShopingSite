using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class product_listController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
