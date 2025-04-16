using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiCrudformViewModelProvider : IViewModelProvider<PoiCrudformViewModel>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapService _mapService;

        public PoiCrudformViewModelProvider(ICategoryService categoryService, IMapService mapService)
        {
            _categoryService = categoryService;
            _mapService = mapService;
        }

        public async Task<PoiCrudformViewModel> Hydrate(PoiCrudformViewModel viewModel)
        {
            Category[] categories = (await _categoryService.Get(null, null)).Value ?? [];
            var map = (await _mapService.Get(null, null)).Value;
            var mapId = map is { Length: > 1 } ? map[0].Id : Guid.Empty;

            return viewModel with
            {
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
