using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface ICategoryService
{
    Task<BackendApiHttpResponse<DomainModels.Category>> Get(string name);
    Task<BackendApiHttpResponse<DomainModels.Category[]>> Get(int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.Category>> CreateSingle(DomainModels.Category poi);
    Task<BackendApiHttpResponse<DomainModels.Category>> UpdateSingle(DomainModels.Category oldVal, DomainModels.Category newVal);
    Task<BackendApiHttpResponse> Delete(DomainModels.Category poi);
}
