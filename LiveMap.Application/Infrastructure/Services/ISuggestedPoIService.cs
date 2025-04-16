using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;
public interface ISuggestedPoIService
{
    Task<BackendApiHttpResponse<DomainModels.SuggestedPointOfInterest>> Get(Guid id);
    Task<BackendApiHttpResponse<DomainModels.SuggestedPointOfInterest[]>> Get(int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.SuggestedPointOfInterest>> CreateSingle(DomainModels.SuggestedPointOfInterest poi);
    Task<BackendApiHttpResponse<DomainModels.SuggestedPointOfInterest>> UpdateSingle(DomainModels.SuggestedPointOfInterest poi);
    Task<BackendApiHttpResponse> Delete(DomainModels.SuggestedPointOfInterest poi);
}
