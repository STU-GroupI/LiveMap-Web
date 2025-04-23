using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Rfc;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("rfc")]
    public class RequestForChangeController : Controller
    {
        [HttpGet("form/{id}")]
        public async Task<IActionResult> ApprovalForm(
            string id,
            [FromServices] IViewModelProvider<RequestForChangeFormViewModel> provider)
        {
            var viewModel = new RequestForChangeFormViewModel(
                Rfc: new() { Id = Guid.Parse(id), SubmittedOn = default, ApprovalStatus = string.Empty }, 
                CrudformViewModel: PoiCrudformViewModel.Empty);

            return View("form", await provider.Hydrate(viewModel));
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(
            [FromQuery] string mapId,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] bool? ascending,
            [FromServices] IViewModelProvider<RequestForChangeListViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new RequestForChangeListViewModel(Guid.Parse(mapId), skip, take, ascending, PaginatedResult<RequestForChange>.Default));
            return View(viewModel);
        }
    }
}
