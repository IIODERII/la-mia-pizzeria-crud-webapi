using la_mia_pizzeria_static;
using Microsoft.AspNetCore.Mvc;

namespace NetCore.Controllers.Api
{    public class FrontController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
