using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Suggestion;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[Controller]")]
    public class SuggestionController : Controller
    {
        private readonly IPointOfInterestService _pointOfInterestService;
        private readonly IRequestForChangeService _requestForChangeService;

        public SuggestionController(IPointOfInterestService pointOfInterestService, IRequestForChangeService requestForChangeService)
        {
            _pointOfInterestService = pointOfInterestService;
            _requestForChangeService = requestForChangeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(
            [FromRoute] string id, 
            [FromServices] IViewModelProvider<SuggestionFormViewModel> provider)
        {
            var viewModel = await provider.Hydrate(SuggestionFormViewModel.EmptyWithId(id));
            return View("SuggestionForm", viewModel);
        }

        [HttpPost("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(
            SuggestionFormViewModel viewModel,
            [FromServices] IViewModelProvider<SuggestionFormViewModel> provider)
        {
            if (!ModelState.IsValid)
            {
                return View("index", await provider.Hydrate(viewModel));
            }

            var rfc = viewModel.ToDomainRequestForChange();
            var poi = viewModel.ToDomainPointOfInterest();

            // TODO: This right here is ritted with business logic. Find a way to get rid of it. For now this is fine, but we need a way around it.
            // Personally I suggest either a handler pattern, local services (which is fine, but prefer them in the application layer), or a re-model.

            var rfcApprovedResult = await _requestForChangeService.Approve(rfc);
            
            // This is basically an early exit. If the rating of the RFC fails, we don't want to create the POI.
            if(!rfcApprovedResult.IsSuccess)
            {
                return await OnServiceError(rfcApprovedResult, provider, viewModel);
            }

            var poiCreatedResult = await _pointOfInterestService.CreateSingle(poi);
            if (!poiCreatedResult.IsSuccess)
            {
                return await OnServiceError(poiCreatedResult, provider, viewModel);
            }

            ViewData["SuccessMessage"] = "The request was successfully processed.";
            return View("SuggestionForm", provider.Hydrate(viewModel));
        }

        [HttpPost("Deny")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(
            SuggestionFormViewModel viewModel,
            [FromServices] IViewModelProvider<SuggestionFormViewModel> provider)
        {
            var rfc = viewModel.ToDomainRequestForChange();
            var rfcApprovedResult = await _requestForChangeService.Deny(rfc);

            if (!rfcApprovedResult.IsSuccess)
            {
                return await OnServiceError(rfcApprovedResult, provider, viewModel);
            }

            ViewData["SuccessMessage"] = "The request was successfully rejected.";
            return View("SuggestionForm", provider.Hydrate(viewModel));
        }

        private async Task<IActionResult> OnServiceError<T>(
            BackendApiHttpResponse<T> response,
            IViewModelProvider<SuggestionFormViewModel> provider,
            SuggestionFormViewModel viewModel)
        {
            ViewData["ErrorMessage"] = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };

            return View("SuggestionForm", await provider.Hydrate(viewModel));
        }
    }
}
