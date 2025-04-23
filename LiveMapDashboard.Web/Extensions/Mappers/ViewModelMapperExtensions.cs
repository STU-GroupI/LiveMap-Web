using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Rfc;
using NetTopologySuite.Simplify;
using System.Runtime.CompilerServices;
using System.Text;
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
            Poi = null!,
        };
    }

    public static OpeningHoursViewModel ToViewModelOpeningHours(this OpeningHours oh)
    {
        var convertTimeValueToString = (TimeSpan time) =>
        {
            StringBuilder sb = new StringBuilder();
            
            if(time.Hours < 10)
            {
                sb.Append('0');
            }
            sb.Append(time.Hours);
            sb.Append(':');
            
            if (time.Minutes < 10)
            {
                sb.Append('0');
            }
            sb.Append(time.Minutes);

            return sb.ToString();
        };

        return new OpeningHoursViewModel(
            IsActive: true,
            Start: convertTimeValueToString(oh.Start),
            End: convertTimeValueToString(oh.End));
    }
    public static List<OpeningHours> ToDomainOpeningHoursList(this ICollection<OpeningHoursViewModel> vm)
    {

        return vm
            .Select((oh, index) => new
            {
                oh.IsActive,
                Oh = oh.IsActive
                        ? oh.ToDomainOpeningHours((DayOfWeek)(index))
                        : null!,
            })
            .Where(item => item.IsActive)
            .Select(item => item.Oh)
            .ToList(); ;
    }

    public static PointOfInterest ToDomainPointOfInterest(this PoiCrudformViewModel viewModel)
    {
        return new()
        {
            Id = viewModel.Id is not null 
                ? Guid.Parse(viewModel.Id) 
                : Guid.Empty,
            Title = viewModel.Title,
            Category = null!,
            CategoryName = viewModel.Category,
            Coordinate = viewModel.Coordinate,
            Description = viewModel.Description,
            Map = null!,
            MapId = Guid.Parse(viewModel.MapId),
            OpeningHours = viewModel.OpeningHours.ToDomainOpeningHoursList(),
            IsWheelchairAccessible = viewModel.IsWheelchairAccessible,
            Status = null!,
            StatusName = string.Empty,
        };
    }
}
