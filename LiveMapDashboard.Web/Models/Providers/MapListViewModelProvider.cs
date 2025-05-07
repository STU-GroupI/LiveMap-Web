using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Park;

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
            PaginatedResult<Map> result = (await _mapService.Get(viewModel.Skip, viewModel.Take)).Value ?? PaginatedResult<Map>.Default;

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
