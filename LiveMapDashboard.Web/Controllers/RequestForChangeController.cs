using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Extensions.Mappers;
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
                Rfc: new() { 
                    Id = Guid.Parse(id), 
                    SubmittedOn = default, 
                    ApprovalStatus = string.Empty 
                }, 
                CrudformViewModel: PoiCrudformViewModel.Empty);

            return View("form", await provider.Hydrate(viewModel));
        }

        [HttpPost("approvalSubmit")]
        public async Task<IActionResult> ApprovalFormSubmit(
            RequestForChangeFormViewModel viewModel,
            [FromServices] IViewModelProvider<RequestForChangeFormViewModel> formProvider,
            [FromServices] IViewModelProvider<RequestForChangeListViewModel> indexProvider,
            [FromServices] IRequestForChangeService requestForChangeService)
        {
            if (!ModelState.IsValid) 
            {
                return View("form", await formProvider.Hydrate(viewModel));
            }
            var result = await requestForChangeService.ApproveRequestForChange(
                viewModel.Rfc, 
                viewModel.CrudformViewModel.ToDomainPointOfInterest());

            return View("index", await indexProvider.Provide());
        }

        [HttpPost("rejectSubmit")]
        public async Task<IActionResult> RejectFormSubmit(
            RequestForChangeFormViewModel viewModel,
            [FromServices] IViewModelProvider<RequestForChangeFormViewModel> formProvider,
            [FromServices] IViewModelProvider<RequestForChangeListViewModel> indexProvider,
            [FromServices] IRequestForChangeService requestForChangeService)
        {
            if (viewModel.Rfc.Id == Guid.Empty)
            {
                return View("form", await formProvider.Hydrate(viewModel));
            }

            var result = await requestForChangeService.RejectRequestForChange(viewModel.Rfc.Id);

            return View("index", await indexProvider.Provide());
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
