using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IPointOfInterestService
{
    Task<ExternalHttpResponse<DomainModels.PointOfInterest>> Get(Guid id);
    Task<ExternalHttpResponse<DomainModels.PointOfInterest[]>> Get(string mapId, int? skip, int? take);
    Task<ExternalHttpResponse<DomainModels.PointOfInterest>> CreateSingle(DomainModels.PointOfInterest poi);
    Task<ExternalHttpResponse<DomainModels.PointOfInterest>> UpdateSingle(DomainModels.PointOfInterest poi);
    Task<ExternalHttpResponse> Delete(Guid id);
}
