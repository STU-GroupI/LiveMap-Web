using DomainModels = LiveMap.Domain.Models;
namespace LiveMap.Api.Models.RequestForChange;

public sealed record ApproveRfcWebRequest(DomainModels.RequestForChange Rfc, DomainModels.PointOfInterest Poi);
