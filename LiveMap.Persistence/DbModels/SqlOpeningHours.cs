namespace LiveMap.Persistence.DbModels;
public class SqlOpeningHours
{
    public required Guid Id { get; set; }

    public required DayOfWeek DayOfWeek { get; set; }

    public required TimeSpan Start { get; set; }
    public required TimeSpan End { get; set; }

    public required Guid PoiId { get; set; }
    public SqlPointOfInterest? Poi { get; set; }
}
