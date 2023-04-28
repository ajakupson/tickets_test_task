using Microsoft.AspNetCore.Mvc;

namespace agileworks_1.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<TaskController> _logger;

        public HomeController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult Home()
        {
            return View("./Views/Home.cshtml");
        }
    }
}