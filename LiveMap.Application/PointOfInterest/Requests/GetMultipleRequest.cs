namespace LiveMap.Application.PointOfInterest.Requests;
public sealed record GetMultipleRequest(Guid MapId, int? Skip, int? Take);