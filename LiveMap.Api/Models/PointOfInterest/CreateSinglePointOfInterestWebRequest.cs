using LiveMap.Domain.Models;

namespace LiveMap.Api.Models.SuggestedPoi;

public record CreateSinglePoiSuggestionWebRequest(
    string Title, 
    string Description, 
    string Category,
    Guid MapId,
    Coordinate Coordinate, 
    bool isWheelchairAccessible);