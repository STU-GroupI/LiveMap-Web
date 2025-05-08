using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Map;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Web.Models.Providers;
using Models = LiveMap.Domain.Models;

public class MapCrudformViewModelProvider : IViewModelProvider<MapCrudformViewModel>
{
    

    private readonly IMapService _mapService;

    public MapCrudformViewModelProvider(IMapService mapService)
    {
        _mapService = mapService;
    }

    public async Task<MapCrudformViewModel> Hydrate(MapCrudformViewModel viewModel)
    {
        if (Guid.TryParse(viewModel.Id, out Guid poiId))
        {
            return await HydratreWithMap(viewModel, poiId);
        }

        return viewModel with
        {
            Id = Guid.Empty.ToString()
        };
    }

    private async Task<MapCrudformViewModel> HydratreWithMap(
        MapCrudformViewModel viewModel,
        Guid mapId
        )
    {
        var mapResult = await _mapService.Get(mapId);

        if (!mapResult.IsSuccess || mapResult.Value is null)
        {
            return viewModel with
            {
                Id = mapId.ToString(),
            };
        }
        Models.Map map = mapResult.Value;

        return viewModel with
        {
            Name = map.Name,
            Area = map.Area,
            //Coordinate = map.,
            Id = mapId.ToString()
        };
    }

    public async Task<MapCrudformViewModel> Provide()
    {
        return await Hydrate(MapCrudformViewModel.Empty);
    }
}
