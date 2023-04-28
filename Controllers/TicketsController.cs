using Microsoft.AspNetCore.Mvc;

namespace agileworks_1.Controllers
{
    [Route("[controller]")]
    public class TicketsController : Controller
    {
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(ILogger<TicketsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult Home()
        {
            return View("./Views/Tickets.cshtml");
        }
    }
}