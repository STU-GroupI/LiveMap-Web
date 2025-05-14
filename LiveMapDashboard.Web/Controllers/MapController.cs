using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Map;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            [FromServices] IViewModelProvider<MapCrudformViewModel> provider,
            MapCrudformViewModel viewModel,
            string action)
        {
            if (!ModelState.IsValid)
            {
                return View("MapForm", await provider.Hydrate(viewModel));
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
                Area = ToCoordinates(viewModel.Area)
            };

            var result = isNewMap
                ? await service.CreateSingle(map)
                : await service.UpdateSingle(map);

            if (isNewMap && result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("MapForm", await provider.Hydrate(viewModel));

        }

        private static Coordinate[] ToCoordinates(string areaJson)
        {
            if (string.IsNullOrEmpty(areaJson))
            {
                return [];
            }

            try
            {
                return JsonSerializer.Deserialize<Coordinate[]>(areaJson) ?? [];
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
