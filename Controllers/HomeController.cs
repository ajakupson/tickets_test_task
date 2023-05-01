using Microsoft.AspNetCore.Mvc;

namespace agileworks_1.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult Home()
        {
            ViewBag.MenuSelection = "home";
            return View("Home");
        }
    }
}