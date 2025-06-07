using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Map;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class MapController : Controller
    {
        public async Task<IActionResult> Index(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromServices] IViewModelProvider<MapListViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new MapListViewModel(skip, take, PaginatedResult<Map>.Default));
            return View(viewModel);
        }

        [HttpGet("form/{id}")]
        public async Task<IActionResult> MapUpdateForm(
            [FromRoute] string? id,
            [FromServices] IViewModelProvider<MapCrudformViewModel> provider)
        {
            if (!Guid.TryParse(id, out _))
            {
                var back = HttpContext?.Request?.Headers?.ContainsKey("Referer") ?? false;

                string backValue = back
                    ? HttpContext!.Request.Headers["Referer"].ToString()
                    : string.Empty;
                return back
                    ? Redirect(backValue)
                    : RedirectToAction(nameof(Index));
            }

            var viewModel = await provider.Hydrate(MapCrudformViewModel.Empty with
            {
                Id = id
            });

            return View("MapForm", viewModel);
        }

        [HttpGet("form")]
        public async Task<IActionResult> MapCreateForm(
            [FromServices] IViewModelProvider<MapCrudformViewModel> provider)
        {
            var viewModel = await provider.Hydrate(MapCrudformViewModel.Empty with
            {
                Id = null
            });
            return View("MapForm", viewModel);
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            [FromServices] IMapService service,
            [FromServices] IImageService imageService,
            [FromServices] IViewModelProvider<MapCrudformViewModel> provider,
            MapCrudformViewModel viewModel,
            string action)
        {
            if (!ModelState.IsValid)
            {
                return View("MapForm", await provider.Hydrate(viewModel));
            }
            
            if (viewModel.ImageFile is IFormFile imageFile)
            {
                var imageResult = await imageService.Create(new(ImageHelpers.ToImage(imageFile)));
                viewModel = viewModel with { ImageUrl = imageResult.Value };
            }

            Guid mapId = Guid.TryParse(viewModel.Id, out Guid outMapId) ? outMapId : Guid.Empty;
            bool isNewMap = mapId == Guid.Empty;
            var map = new Map()
            {
                Id = mapId,
                Name = viewModel.Name,
                ImageUrl = viewModel.ImageUrl,
                Bounds =
                [
                    viewModel.TopLeft,
                    viewModel.TopRight,
                    viewModel.BottomLeft,
                    viewModel.BottomRight
                ],
                Area = MapCrudformViewModel.ToCoordinates(viewModel.Area)
            };

            var result = isNewMap
                ? await service.CreateSingle(map)
                : await service.UpdateSingle(map);

            if (isNewMap)
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

            return View("MapForm", await provider.Hydrate(viewModel));
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
