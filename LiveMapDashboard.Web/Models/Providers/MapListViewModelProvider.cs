using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Map;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class MapListViewModelProvider : IViewModelProvider<MapListViewModel>
    {
        private readonly IMapService _mapService;

        public MapListViewModelProvider(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<MapListViewModel> Hydrate(MapListViewModel viewModel)
        {
            var mapsResult = await _mapService.Get(null, null);
            var maps = mapsResult.Value?.Items ?? [];

            return viewModel with
            {
                Maps = maps.Select(map => new MapListEntryViewModel()
                {
                    Id = map.Id.ToString(),
                    Name = map.Name
                }).ToList()
            };
        }

        public async Task<MapListViewModel> Provide()
        {
            var viewModel = new MapListViewModel();

            return await Hydrate(viewModel);
        }
    }
}
