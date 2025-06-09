using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Pagination;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IMapService
{
    Task<ExternalHttpResponse<DomainModels.Map>> Get(Guid id);
    Task<ExternalHttpResponse<PaginatedResult<DomainModels.Map>>> Get(int? skip, int? take);
    Task<ExternalHttpResponse<DomainModels.Map>> CreateSingle(DomainModels.Map map);
    Task<ExternalHttpResponse<DomainModels.Map>> UpdateSingle(DomainModels.Map map);
    Task<ExternalHttpResponse> Delete(Guid id);
}