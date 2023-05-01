using Microsoft.AspNetCore.Mvc;

namespace agileworks_1.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ViewResult Home()
        {
            ViewBag.MenuSelection = "task";
            return View("Task");
        }
    }
}