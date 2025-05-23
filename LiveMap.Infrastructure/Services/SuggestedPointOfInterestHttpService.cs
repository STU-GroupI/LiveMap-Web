using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;

namespace LiveMap.Infrastructure.Services;

public class SuggestedPointOfInterestHttpService : ISuggestedPointOfInterestService
{
    private readonly IBackendApiHttpService _backendApiHttpService;
    private const string _ENDPOINT = "rfc/poisuggestion";

    public SuggestedPointOfInterestHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiHttpService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<SuggestedPointOfInterest>> Get(Guid id)
    {
        return await _backendApiHttpService.SendRequest<SuggestedPointOfInterest>(new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
        });
    }
}
