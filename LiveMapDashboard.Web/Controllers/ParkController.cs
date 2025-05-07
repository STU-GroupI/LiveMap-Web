using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Park;
using Microsoft.AspNetCore.Mvc;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class ParkController : Controller
    {
        public async Task<IActionResult> Index(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromServices] IViewModelProvider<MapListViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new MapListViewModel(skip, take, PaginatedResult<Map>.Default));
            return View(viewModel);
        }
    }
}
