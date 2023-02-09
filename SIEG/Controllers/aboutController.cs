using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class aboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
