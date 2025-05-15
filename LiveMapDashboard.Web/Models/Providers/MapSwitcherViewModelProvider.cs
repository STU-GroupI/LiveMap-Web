using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Models.Dashboard;

namespace LiveMapDashboard.Web.Models.Providers;

public class MapSwitcherViewModelProvider : IViewModelProvider<MapSwitcherViewModel>
{
    private readonly IMapService _mapService;

    public MapSwitcherViewModelProvider(IMapService mapService)
    {
        _mapService = mapService;
    }

    public async Task<MapSwitcherViewModel> Hydrate(MapSwitcherViewModel viewModel)
    {
        // The reason this is a valuetask is because we want to limit the amount of allocations
        // we are doing here. ValueTasks are like tasks. They allocate less heap memory, but you HAVE
        // to know what you are doing as using them does not come without consequence, so be warned.
        Func<ValueTask<string?>> getMapName = async () =>
        {
            if (viewModel.MapId is null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(viewModel.MapName))
            {
                return viewModel.MapName;
            }

            var serviceResult = await _mapService.Get(viewModel.MapId.Value);
            if (!serviceResult.IsSuccess || serviceResult.Value is null)
            {
                return null;
            }

            return serviceResult.Value.Name;
        };

        IEnumerable<(string id, string name)> maps = await _mapService.Get(null, null) switch
        {
            var result when result is { IsSuccess: true, Value: { Length: >0 } } 
                => result.Value
                    .Select(map => (map.Id.ToString(), map.Name))
                    .AsEnumerable(),

            _ => Enumerable.Empty<(string id, string name)>()
        };

        return viewModel with
        {
            Maps = maps,
            MapName = await getMapName()
        };
    }

    public async Task<MapSwitcherViewModel> Provide()
    {
        return await Hydrate(MapSwitcherViewModel.Empty);
    }
}
