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
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider)
        {
            var viewModel = await provider.Provide();
            return View(viewModel);
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            [FromServices] IPointOfInterestService service,
            [FromServices] IViewModelProvider<PoiCrudformViewModel> provider,
            PoiCrudformViewModel viewModel)
        {
            var poi = viewModel.ToDomainPointOfInterest();
            var result = await service.Delete(poi.Id);

            if (result.IsSuccess)
            {
                ViewData["SuccessMessage"] = "The point of interest and its contents where successfully deleted!";
                return View("index", await provider.Provide());
            }
            
            ViewData["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };
            
            return View("index", await provider.Hydrate(viewModel));
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
                return View("index", await provider.Hydrate(viewModel));
            }

            var poi = viewModel.ToDomainPointOfInterest();
            var result = await service.CreateSingle(poi);

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

            return View("index", await provider.Hydrate(viewModel));
        }
    }
}
