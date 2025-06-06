using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Pagination;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IRequestForChangeService
{
    Task<ExternalHttpResponse<DomainModels.RequestForChange>> Get(Guid id);
    Task<ExternalHttpResponse<PaginatedResult<DomainModels.RequestForChange>>> GetMultiple(Guid? mapId, int? skip, int? take, bool? ascending, bool? approved);

    Task<ExternalHttpResponse> ApproveRequestForChange(DomainModels.RequestForChange rfc, DomainModels.PointOfInterest poi);
    Task<ExternalHttpResponse> RejectRequestForChange(Guid rfcId);
}
