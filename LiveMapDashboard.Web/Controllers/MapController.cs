using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Map;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class MapController : Controller
    {
        public async Task<IActionResult> Index(
            [FromServices] IViewModelProvider<MapListViewModel> provider)
        {
            var viewModel = await provider.Provide();
            return View(viewModel);
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            [FromServices] IMapService service,
            MapDeleteViewModel viewModel)
        {
            var mapId = viewModel.Id is not null
                ? Guid.Parse(viewModel.Id)
                : Guid.Empty;
            
            if (mapId == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "The map was not found.";
                return RedirectToAction(nameof(Index));
            }
            
            var result = await service.Delete(mapId);
            
            if (result.IsSuccess)
            {
                ViewData["SuccessMessage"] = "The map and its contents where successfully deleted!";
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };

            return RedirectToAction(nameof(Index));
        }
    }
}
