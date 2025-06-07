namespace LiveMap.Application.RequestForChange.Requests;

public sealed record GetMultipleRequest(Guid MapId, int? Skip, int? Take, bool? Ascending, bool? IsPending);
