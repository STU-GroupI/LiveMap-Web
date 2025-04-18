using LiveMap.Domain.Pagination;

namespace LiveMap.Application.RequestForChange.Responses;

public sealed record GetMultipleResponse(PaginatedResult<Domain.Models.RequestForChange> Rfc);
