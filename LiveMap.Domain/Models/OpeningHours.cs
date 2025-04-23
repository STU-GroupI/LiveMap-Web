namespace LiveMap.Domain.Models;

public class OpeningHours
{
    public required Guid Id { get; set; }

    public required DayOfWeek DayOfWeek { get; set; }

    public required TimeSpan Start { get; set; }
    public required TimeSpan End { get; set; }

    public required Guid PoiId { get; set; }
    public PointOfInterest? Poi { get; set; }
}
