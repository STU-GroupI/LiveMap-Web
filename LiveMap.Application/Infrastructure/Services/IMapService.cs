using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Pagination;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IMapService
{
    Task<BackendApiHttpResponse<DomainModels.Map>> Get(Guid id);
    Task<BackendApiHttpResponse<PaginatedResult<DomainModels.Map>>> Get(int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.Map>> CreateSingle(DomainModels.Map map);
    Task<BackendApiHttpResponse<DomainModels.Map>> UpdateSingle(DomainModels.Map map);
    Task<BackendApiHttpResponse> Delete(Guid id);
}