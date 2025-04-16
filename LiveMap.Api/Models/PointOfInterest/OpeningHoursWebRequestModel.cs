namespace LiveMap.Api.Models.PointOfInterest;

public record OpeningHoursWebRequestModel(
    DayOfWeek DayOfWeek,
    TimeSpan Start,
    TimeSpan End
);