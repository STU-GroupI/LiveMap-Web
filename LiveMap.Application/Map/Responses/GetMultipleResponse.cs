using LiveMap.Domain.Pagination;

namespace LiveMap.Application.Map.Responses;
public sealed record GetMultipleResponse(PaginatedResult<Domain.Models.Map> Result);
