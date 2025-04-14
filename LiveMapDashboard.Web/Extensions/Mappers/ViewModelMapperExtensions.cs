using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;

namespace LiveMapDashboard.Web.Extensions.Mappers;

public static class ViewModelMapperExtensions
{
    public static OpeningHours ToDomainOpeningHours(this OpeningHoursViewModel vm, DayOfWeek dayOfWeek)
    {
        var toTime = (string time) =>
        {
            var split = time.Split(':');
            
            if(!int.TryParse(split[0], out var hours) || !int.TryParse(split[1], out var minutes))
            {
                throw new FormatException($"Open and closing times must be formatted HH:MM, not: {time}");
            }

            return TimeSpan.FromHours(hours: hours, minutes: minutes);
        };
        return new()
        {
            Id = Guid.Empty,
            PoiId = Guid.Empty,
            DayOfWeek = dayOfWeek,
            Start = toTime(vm.Start),
            End = toTime(vm.End),
        };
    }
    public static List<OpeningHours> ToDomainOpeningHoursList(this ICollection<OpeningHoursViewModel> vm)
    {
        return vm.Select((oh, index) => 
            oh.ToDomainOpeningHours((DayOfWeek)(index == 0 ? 6 : index - 1)))
            .ToList();
    }

    public static PointOfInterest ToDomainPointOfInterest(this PoiCrudformViewModel viewModel)
    {
        return new()
        {
            Title = viewModel.Title,
            Category = null!,
            CategoryName = viewModel.Category,
            Coordinate = viewModel.Coordinate,
            Description = viewModel.Description,
            Id = Guid.Empty,
            Map = null!,
            MapId = Guid.Parse(viewModel.MapId),
            OpeningHours = viewModel.OpeningHours.ToDomainOpeningHoursList(),
            IsWheelchairAccessible = viewModel.IsWheelchairAccessible,
            Status = null!,
            StatusName = string.Empty,
        };
    }
}
