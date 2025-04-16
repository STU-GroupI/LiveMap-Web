using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Suggestion
{
    public record SuggestionFormViewModel(
        string Title,
        string Category,
        string Description,
        bool IsWheelchairAccessible,
        string MapId,
        Coordinate Coordinate,
        OpeningHoursViewModel[] OpeningHours,
        Category[]? Categories,
        string RfcId,
        string Message,
        string ApprovalStatus
    ) : PoiCrudformViewModel(
        Title,
        Category,
        Description,
        IsWheelchairAccessible,
        MapId,
        Coordinate,
        OpeningHours,
        Categories)
    {
        public static SuggestionFormViewModel EmptyWithId(string rfcId) => new SuggestionFormViewModel(
            Title: string.Empty,
            Category: string.Empty,
            Description: string.Empty,
            IsWheelchairAccessible: false,
            MapId: string.Empty,
            Coordinate: new(0, 0),
            OpeningHours: Enumerable.Repeat(OpeningHoursViewModel.Empty, 7).ToArray(),
            Categories: [],
            RfcId: rfcId,
            Message: string.Empty,
            ApprovalStatus: string.Empty);
    }
}