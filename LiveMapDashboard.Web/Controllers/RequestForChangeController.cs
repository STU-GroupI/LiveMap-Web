using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Rfc;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class RequestForChangeController : Controller
    {
        [HttpGet("form/{id}")]
        public async Task<IActionResult> ApprovalForm(
            [FromRoute] string id)
        {
            return View("form");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(
            [FromRoute] string id,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] bool? ascending,
            [FromServices] IViewModelProvider<RequestForChangeListViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new RequestForChangeListViewModel(Guid.Parse(id), skip, take, ascending, PaginatedResult<RequestForChange>.Default));
            return View(viewModel);
        }
    }
}
