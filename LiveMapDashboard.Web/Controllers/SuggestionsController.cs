using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ILogger<SuggestionsController> _logger;

        public SuggestionsController(ILogger<SuggestionsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Single()
        {
            return View();
        }
    }
}
