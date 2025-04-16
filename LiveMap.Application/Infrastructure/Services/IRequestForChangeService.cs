using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;
public interface IRequestForChangeService
{
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> Get(string name);
    Task<BackendApiHttpResponse<DomainModels.RequestForChange[]>> Get(int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> CreateSingle(DomainModels.RequestForChange rfc);
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> UpdateSingle(DomainModels.RequestForChange rfc);
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> Approve(DomainModels.RequestForChange rfc);
    Task<BackendApiHttpResponse<DomainModels.RequestForChange>> Deny(DomainModels.RequestForChange rfc);
    Task<BackendApiHttpResponse> Delete(DomainModels.RequestForChange rfc);
}
