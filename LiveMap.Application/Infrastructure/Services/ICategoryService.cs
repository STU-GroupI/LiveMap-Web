using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface ICategoryService
{
    Task<BackendApiHttpResponse<DomainModels.Category>> GetSingle(Guid id);
    Task<BackendApiHttpResponse<DomainModels.Category>> GetAll();
    Task<BackendApiHttpResponse<DomainModels.Category>> CreateSingle(DomainModels.Category poi);
    Task<BackendApiHttpResponse<DomainModels.Category>> UpdateSingle(DomainModels.Category poi);
    Task<BackendApiHttpResponse> Delete(DomainModels.Category poi);
}
