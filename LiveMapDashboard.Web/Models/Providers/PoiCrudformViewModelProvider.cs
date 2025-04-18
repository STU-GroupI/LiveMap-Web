using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiCrudformViewModelProvider : IViewModelProvider<PoiCrudformViewModel>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapService _mapService;
        private readonly IPointOfInterestService _pointOfInterestService;

        public PoiCrudformViewModelProvider(ICategoryService categoryService,
                                            IMapService mapService,
                                            IPointOfInterestService pointOfInterestService)
        {
            _categoryService = categoryService;
            _mapService = mapService;
            _pointOfInterestService = pointOfInterestService;
        }

        public async Task<PoiCrudformViewModel> Hydrate(PoiCrudformViewModel viewModel)
        {
            Category[] categories = (await _categoryService.Get(null, null)).Value ?? [];
            var map = (await _mapService.Get(null, null)).Value;
            var mapId = map is { Length: > 1 } ? map[0].Id : Guid.Empty;

            if(Guid.TryParse(viewModel.Id, out var poiId))
            {
                return await HydrateWithPoi(viewModel, poiId, categories, mapId);
            }

            return viewModel with
            {
                Categories = categories,
                MapId = mapId.ToString(),
            };
        }

        private async Task<PoiCrudformViewModel> HydrateWithPoi(
            PoiCrudformViewModel viewModel,
            Guid poiId,
            Category[] categories,
            Guid mapId)
        {
            var poiResult = await _pointOfInterestService.Get(poiId);

            if(!poiResult.IsSuccess || poiResult.Value is null)
            {
                return viewModel with
                {
                    Categories = categories,
                    MapId = mapId.ToString(),
                };
            }
            PointOfInterest poi = poiResult.Value;

            return viewModel with
            {
                Title = poi.Title,
                Category = poi.CategoryName,
                Coordinate = poi.Coordinate,
                Description = poi.Description,
                IsWheelchairAccessible = poi.IsWheelchairAccessible,
                OpeningHours = poi.OpeningHours
                    .Select(oh => oh.ToViewModelOpeningHours())
                    .ToArray(),
                Categories = categories,
                MapId = mapId.ToString(),
            };
        }

        public async Task<PoiCrudformViewModel> Provide()
        {
            return await Hydrate(PoiCrudformViewModel.Empty);
        }
    }
}
