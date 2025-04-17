using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Providers
{
    public class PoiListViewModelProvider : IViewModelProvider<PoiListViewModel>
    {
        private readonly IMapService _mapService;
        private readonly IPointOfInterestService _pointOfInterestService;

        public PoiListViewModelProvider(IMapService mapService, IPointOfInterestService pointOfInterestService)
        {
            _mapService = mapService;
            _pointOfInterestService = pointOfInterestService;
        }

        public async Task<PoiListViewModel> Provide()
        {
            var viewModel = new PoiListViewModel();

            return await Hydrate(viewModel);
        }

        public async Task<PoiListViewModel> Hydrate(PoiListViewModel viewModel)
        {
            var mapsResult = await _mapService.Get(0, 1);
            var map = mapsResult.Value?.FirstOrDefault();

            var poisResult = await _pointOfInterestService.Get(map!.Id.ToString(), null, null);
            var pois = poisResult.Value ?? [];

            return viewModel with
            {
                Pois = pois.Select(poi => new PoiListEntryViewModel
                {
                    Id = poi.Id.ToString(),
                    Name = poi.Title
                }).ToList()
            };
        }
    }
}