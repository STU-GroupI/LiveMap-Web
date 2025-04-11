namespace LiveMap.Application.PointOfInterest.Responses;
public sealed record GetMultipleResponse(ICollection<Domain.Models.PointOfInterest> PointsOfInterests);
