using LiveMap.Application.Infrastructure.Models;
using DomainModels = LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IRequestForChangeService
{
    Task<BackendApiHttpResponse<DomainModels.RequestForChange[]>> Get(Guid mapId, int? skip, int? take, bool? ascending);
}
