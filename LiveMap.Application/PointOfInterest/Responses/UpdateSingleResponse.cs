using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Requests;

public sealed record UpdateSingleResponse(Models.PointOfInterest? Poi);