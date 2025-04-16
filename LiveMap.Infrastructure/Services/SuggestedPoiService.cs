using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;

namespace LiveMap.Infrastructure.Services;
public class SuggestedPoiService : ISuggestedPoIService
{
    private readonly IBackendApiHttpService _backendApiService;
    private const string _ENDPOINT = "rfc/poisuggestion";

    public SuggestedPoiService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public Task<BackendApiHttpResponse<SuggestedPointOfInterest>> CreateSingle(SuggestedPointOfInterest poi)
    {
        throw new NotImplementedException();
    }

    public Task<BackendApiHttpResponse> Delete(SuggestedPointOfInterest poi)
    {
        throw new NotImplementedException();
    }

    public async Task<BackendApiHttpResponse<SuggestedPointOfInterest>> Get(Guid id)
    {
        return await _backendApiService
            .SendRequest<SuggestedPointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            });
    }

    public Task<BackendApiHttpResponse<SuggestedPointOfInterest[]>> Get(int? skip, int? take)
    {
        throw new NotImplementedException();
    }

    public Task<BackendApiHttpResponse<SuggestedPointOfInterest>> UpdateSingle(SuggestedPointOfInterest poi)
    {
        throw new NotImplementedException();
    }
}
