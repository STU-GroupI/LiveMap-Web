using LiveMap.Domain.Models;

namespace LiveMap.Api.Models;

public sealed record UpdateSingleRfcWebRequest(Guid Id, string ApprovalStatus, string Message, DateTime ApprovedOn);