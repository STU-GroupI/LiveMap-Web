using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Responses;

public sealed record UpdateSingleResponse(Models.PointOfInterest? Poi);