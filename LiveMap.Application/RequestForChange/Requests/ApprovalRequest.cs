namespace LiveMap.Application.RequestForChange.Requests;
using Domain.Models;

public sealed record ApprovalRequest(RequestForChange Rfc, PointOfInterest Poi);