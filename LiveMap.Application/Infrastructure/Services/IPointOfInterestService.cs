using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IPointOfInterestService
{
    Task<BackendApiHttpResponse<DomainModels.PointOfInterest>> Get(Guid id);
    Task<BackendApiHttpResponse<DomainModels.PointOfInterest[]>> Get(string mapId, int? skip, int? take);
    Task<BackendApiHttpResponse<DomainModels.PointOfInterest>> CreateSingle(DomainModels.PointOfInterest poi);
    Task<BackendApiHttpResponse<DomainModels.PointOfInterest>> UpdateSingle(DomainModels.PointOfInterest poi);
    Task<BackendApiHttpResponse<DomainModels.PointOfInterest>> Delete(Guid id);
}
