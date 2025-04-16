using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.Models.Suggestion
{
    public record SuggestionFormViewModel : PoiCrudformViewModel
    {
        public string RfcId { get; init; }
        public string Message { get; init; }
        public string ApprovalStatus { get; init; }

        public SuggestionFormViewModel(string Title,
                                           string Category,
                                           string Description,
                                           bool IsWheelchairAccessible,
                                           string MapId,
                                           Coordinate Coordinate,
                                           OpeningHoursViewModel[] OpeningHours,
                                           Category[]? Categories,
                                           string rfcId,
                                           string message,
                                           string approvalStatus) : base(Title, Category, Description, IsWheelchairAccessible, MapId, Coordinate, OpeningHours, Categories)
        {
            ApprovalStatus = approvalStatus;
            Message = message;
            RfcId = rfcId;
        }

        public static SuggestionFormViewModel EmptyWithId(string rfcId) => new SuggestionFormViewModel(
            Title: string.Empty,
            Category: string.Empty,
            Description: string.Empty,
            IsWheelchairAccessible: false,
            MapId: string.Empty,
            Coordinate: new(0, 0),
            OpeningHours: Enumerable.Repeat(OpeningHoursViewModel.Empty, 7).ToArray(),
            Categories: [],
            rfcId: rfcId,
            message: string.Empty,
            approvalStatus: string.Empty);
    }
}