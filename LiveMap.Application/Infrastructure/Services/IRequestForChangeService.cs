using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Pagination;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IRequestForChangeService
{
    Task<BackendApiHttpResponse<PaginatedResult<DomainModels.RequestForChange>>> Get(Guid mapId, int? skip, int? take, bool? ascending);
}
