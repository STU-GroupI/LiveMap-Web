using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class PoiController : Controller
    {
        public async Task<IActionResult> Index(
            [FromServices] IViewModelProvider<PoiListViewModel> provider)
        {
            var viewModel = await provider.Provide();
            return View(viewModel);
        }

        [HttpGet("form/{id}")]
        public async Task<IActionResult> PoiUpdateForm(
            [FromRoute] string? id,
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider)
        {
            if (!Guid.TryParse(id, out var poiId))
            {
                var back = HttpContext?.Request?.Headers?.ContainsKey("Referer") ?? false;

                string backValue = back
                    ? HttpContext!.Request.Headers["Referer"].ToString()
                    : string.Empty;
                return back
                    ? Redirect(backValue)
                    : RedirectToAction(nameof(Index));

            }

            var viewModel = await provider.Hydrate(PoiCrudformViewModel.Empty with
            {
                Id = id
            });

            return View("PoiForm", viewModel);
        }

        [HttpGet("form")]
        public async Task<IActionResult> PoiCreateForm(
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider)
        {
            var viewModel = await provider.Hydrate(PoiCrudformViewModel.Empty with
            {
                Id = null
            });
            return View("PoiForm", viewModel);
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            [FromServices] IPointOfInterestService service,
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider,
            PoiCrudformViewModel viewModel)
        {
            var poiId = viewModel.Id is not null
                ? Guid.Parse(viewModel.Id)
                : Guid.Empty;

            if (poiId == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "The point of interest was not found.";
                return View("PoiForm", await provider.Provide());
            }

            var result = await service.Delete(poiId);

            if (result.IsSuccess)
            {
                ViewData["SuccessMessage"] = "The point of interest and its contents where successfully deleted!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };

            return View("PoiForm", await provider.Provide());
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            [FromServices] IPointOfInterestService service,
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider,
            PoiCrudformViewModel viewModel,
            string action)
        {
            if (!ModelState.IsValid)
            {
                return View("PoiForm", await provider.Hydrate(viewModel));
            }

            var poi = viewModel.ToDomainPointOfInterest();
            var isNewPoi = poi.Id == Guid.Empty || poi.Id.ToString() == string.Empty;

            var result = isNewPoi
                ? await service.CreateSingle(poi)
                : await service.UpdateSingle(poi);

            if (isNewPoi)
            {
                return RedirectToAction(nameof(Index));
            }

            if (result.IsSuccess)
            {
                ViewData["SuccessMessage"] = "Your request was successfully processed!";
            }
            else
            {
                ViewData["ErrorMessage"] = result.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                    _ => "Something went wrong while trying to contact the application. Please try again later"
                };
            }

            return View("PoiForm", await provider.Hydrate(viewModel));
        }
    }
}
