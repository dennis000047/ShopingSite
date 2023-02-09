using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SIEG.Controllers
{
    public class product_infoController : Controller
    {
        public IActionResult index()
        {
            return View();
        }
        public IActionResult sell()
        {
            return View();
        }

        public IActionResult order()
        {
            return View();
        }
    }
}
