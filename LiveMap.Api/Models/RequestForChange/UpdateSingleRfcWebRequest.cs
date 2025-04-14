using LiveMap.Domain.Models;

namespace LiveMap.Api.Models;

public sealed record UpdateSingleRfcWebRequest(Guid PoiId, ApprovalStatus? ApprovalStatus, RequestForChange RequestForChange);