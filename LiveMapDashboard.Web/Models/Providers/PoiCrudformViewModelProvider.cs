﻿using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Providers;

using Models = LiveMap.Domain.Models;
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
        Models.Category[] categories = (await _categoryService.Get(null, null)).Value ?? [];

        if (!Guid.TryParse(viewModel.Id, out var poiId))
        {
            return viewModel with
            {
                Categories = categories
            };
        }

        return await HydrateWithPoi(viewModel, poiId, categories);
    }

    private OpeningHoursViewModel[] NormalizeOpeningHours(List<OpeningHours> existingHours)
    {
        var allDays = Enum.GetValues<DayOfWeek>();

        // Convert to dictionary for faster lookup
        var existingDict = existingHours.ToDictionary(oh => oh.DayOfWeek, oh => oh);

        var result = new List<OpeningHoursViewModel>();

        foreach (var day in allDays)
        {
            if (existingDict.TryGetValue(day, out var existing))
            {
                result.Add(existing.ToViewModelOpeningHours());
            }
            else
            {
                result.Add(new OpeningHoursViewModel
                (
                    Start: new TimeOnly(9, 0).ToString(),
                    End: new TimeOnly(17, 0).ToString(),
                    IsActive: false
                ));
            }
        }

        return result.ToArray();
    }

    private async Task<PoiCrudformViewModel> HydrateWithPoi(
        PoiCrudformViewModel viewModel,
        Guid poiId,
        Models.Category[] categories)
    {
        var poiResult = await _pointOfInterestService.Get(poiId);

        if (poiResult is not { IsSuccess: true, Value: not null })
        {
            throw new Exception($"The poi for the given ID was not found while hydrating from {this.GetType().FullName}");
        }

        PointOfInterest poi = poiResult.Value;

        var openingHours = poi.OpeningHours?
                .Select(oh => oh.ToViewModelOpeningHours())
                .ToArray() ?? [];

        return viewModel with
        {
            Title = poi.Title,
            Category = poi.CategoryName,
            Image = poi.Image,
            Coordinate = poi.Coordinate,
            Description = poi.Description,
            IsWheelchairAccessible = poi.IsWheelchairAccessible,
            OpeningHours = this.NormalizeOpeningHours(poi.OpeningHours?.ToList() ?? []),
            Categories = categories,
            MapId = poi.MapId.ToString(),
        };
    }

    public async Task<PoiCrudformViewModel> Provide()
    {
        return await Hydrate(PoiCrudformViewModel.Empty);
    }
}