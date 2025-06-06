using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface ICategoryService
{
    Task<ExternalHttpResponse<DomainModels.Category>> Get(string name);
    Task<ExternalHttpResponse<DomainModels.Category[]>> Get(int? skip, int? take);
    Task<ExternalHttpResponse<DomainModels.Category>> CreateSingle(DomainModels.Category poi);
    Task<ExternalHttpResponse<DomainModels.Category>> UpdateSingle(DomainModels.Category oldVal, DomainModels.Category newVal);
    Task<ExternalHttpResponse> Delete(DomainModels.Category poi);
}
