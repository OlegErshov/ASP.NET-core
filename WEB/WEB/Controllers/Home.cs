using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
