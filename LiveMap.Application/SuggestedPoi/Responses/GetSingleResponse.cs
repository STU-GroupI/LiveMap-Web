using LiveMap.Domain.Models;

namespace LiveMap.Application.SuggestedPoi.Responses;

public sealed record GetSingleResponse(SuggestedPointOfInterest? SuggestedPoi);
