using LiveMap.Domain.Models;

namespace LiveMapDashboard.Web.Models.Poi;

public sealed record PoiCrudformViewModel(
    string Title, 
    string Category, 
    string Description, 
    bool IsWheelchairAccessible,
    string ParkId, 
    Coordinate Coordinate,
    OpeningHoursViewModel[] OpeningHours,
    Category[] Categories)
{
    public static PoiCrudformViewModel Empty => 
        new PoiCrudformViewModel(
            Title: string.Empty,
            Category: string.Empty,
            Description: string.Empty,
            IsWheelchairAccessible: false,
            ParkId: string.Empty,
            Coordinate: new(0, 0),
            OpeningHours: new OpeningHoursViewModel[7],
            Categories: []);
}
