namespace LiveMap.Api.Models;

public sealed record CreateSingleRfcWebRequest(Guid? PoiId, Guid? SuggestedPoiId, string Message);
