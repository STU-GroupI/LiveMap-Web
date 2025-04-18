using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Suggestions;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class SuggestionsController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(
            [FromRoute] string id,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] bool? ascending,
            [FromServices] IViewModelProvider<PoiSuggestionsViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new PoiSuggestionsViewModel(Guid.Parse(id), skip, take, ascending, []));
            return View(viewModel);
        }
    }
}
