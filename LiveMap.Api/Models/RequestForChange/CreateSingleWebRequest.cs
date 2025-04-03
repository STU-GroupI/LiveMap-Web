namespace LiveMap.Api.Models;

public sealed record CreateSingleWebRequest(Guid? PoiId, Guid? SuggestedPoiId, string Message);
