using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Exceptions;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Providers;

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
        var mapsResult = await _mapService.Get(viewModel.MapId);

        if(mapsResult is not { IsSuccess: true, Value: not null })
        {
            throw new MapNotFoundException(viewModel.MapId.ToString());
        }

        var map = mapsResult.Value;

        var poisResult = await _pointOfInterestService.Get(map.Id.ToString(), null, null);
        var pois = poisResult.Value ?? [];

        return viewModel with
        {
            Pois = pois.Select(poi => new PoiListEntryViewModel()
            {
                Id = poi.Id.ToString(),
                Name = poi.Title
            }).ToList()
        };
    }
}