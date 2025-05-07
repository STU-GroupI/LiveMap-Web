using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Pagination;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IRequestForChangeService
{
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> Get(Guid id);
    Task<BackendApiHttpResponse<PaginatedResult<DomainModels.RequestForChange>>> GetMultiple(Guid? mapId, int? skip, int? take, bool? ascending);

    Task<BackendApiHttpResponse> ApproveRequestForChange(DomainModels.RequestForChange rfc, DomainModels.PointOfInterest poi);
}
