using LiveMap.Application.Infrastructure.Models;
using LiveMap.Domain.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface ISuggestedPointOfInterestService
{
    public Task<BackendApiHttpResponse<SuggestedPointOfInterest>> Get(Guid id);
}
