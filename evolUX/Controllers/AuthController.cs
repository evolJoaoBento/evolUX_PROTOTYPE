using Microsoft.AspNetCore.Mvc;

namespace evolUX.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
