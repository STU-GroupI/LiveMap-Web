using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Map;


namespace LiveMapDashboard.Web.Models.Providers
{
    using Models = LiveMap.Domain.Models;

    public class MapListViewModelProvider : IViewModelProvider<MapListViewModel>
    {
        private readonly IMapService _mapService;

        public MapListViewModelProvider(IMapService mapService)
        {
            _mapService = mapService;
        }

        public async Task<MapListViewModel> Hydrate(MapListViewModel viewModel)
        {
            PaginatedResult<Models.Map> result = (await _mapService.Get(viewModel.Skip, viewModel.Take)).Value ?? PaginatedResult<Models.Map>.Default;

            return viewModel with
            {
                Result = result
            };
        }

        public async Task<MapListViewModel> Provide()
        {
            return await Hydrate(MapListViewModel.Empty);
        }
    }
}
