using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface ISuggestedPointOfInterestService
{
    Task<BackendApiHttpResponse<DomainModels.SuggestedPointOfInterest[]>> Get(Guid mapId, int? skip, int? take, bool? ascending);
}
