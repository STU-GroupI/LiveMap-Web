﻿using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Extensions;
using LiveMapDashboard.Web.Models.Map;

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
            return await HydrateWithMap(viewModel, poiId);
        }

        return viewModel with
        {
            Id = Guid.Empty.ToString()
        };
    }

    private async Task<MapCrudformViewModel> HydrateWithMap(
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
            TopLeft = GetBound(map, 0),
            TopRight = GetBound(map, 1),
            BottomLeft = GetBound(map, 2),
            BottomRight = GetBound(map, 3),
            Id = mapId.ToString(),
            ImageUrl = map.ImageUrl,
            Area = map.Area.ToJsonCoordinates()
        };
    }

    public Coordinate GetBound(Models.Map map, int index)
    {
        return map.Bounds[index] ?? new Coordinate(0.0, 0.0);
    }

    public async Task<MapCrudformViewModel> Provide()
    {
        return await Hydrate(MapCrudformViewModel.Empty);
    }
}
