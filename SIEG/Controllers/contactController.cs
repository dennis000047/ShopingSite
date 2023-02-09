using Microsoft.AspNetCore.Mvc;

namespace SIEG.Controllers
{
    public class contactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
