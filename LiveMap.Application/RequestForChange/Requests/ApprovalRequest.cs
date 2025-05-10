using DomainModels = LiveMap.Domain.Models;
namespace LiveMap.Application.RequestForChange.Requests;

public sealed record ApprovalRequest(DomainModels.RequestForChange Rfc, DomainModels.PointOfInterest Poi);