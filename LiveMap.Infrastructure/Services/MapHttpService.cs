using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;

namespace LiveMap.Infrastructure.Services;
public class MapHttpService : IMapService
{
    private const string _ENDPOINT = "map";
    private readonly IBackendApiHttpService _backendApiService;

    public MapHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public Task<BackendApiHttpResponse<Map>> CreateSingle(Category poi)
    {
        throw new NotImplementedException();
    }

    public Task<BackendApiHttpResponse> Delete(Category poi)
    {
        throw new NotImplementedException();
    }

    public async Task<BackendApiHttpResponse<Map>> Get(Guid id)
    {
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            });
    }

    public async Task<BackendApiHttpResponse<Map[]>> Get(int? skip, int? take)
    {
        var query = $"{nameof(skip)}={skip}&{nameof(take)}={take}";
        var uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<Map[]>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }


    public Task<BackendApiHttpResponse<Map>> UpdateSingle(Category poi)
    {
        throw new NotImplementedException();
    }
}