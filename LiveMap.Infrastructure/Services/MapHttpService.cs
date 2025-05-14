using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System.Text;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services;
public class MapHttpService : IMapService
{
    private const string _ENDPOINT = "map";
    private readonly IBackendApiHttpService _backendApiService;

    public MapHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<Map>> CreateSingle(Map map)
    {
        //await Task.Yield(); // Simulate async, remove when method is implemented and add async to method
        //throw new NotImplementedException();

        // Setup for Backend communication
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(map), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_ENDPOINT, UriKind.Relative)
            });
    }

    public Task<BackendApiHttpResponse> Delete(Map map)
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

    public async Task<BackendApiHttpResponse<PaginatedResult<Map>>> Get(int? skip, int? take)
    {
        var query = $"{nameof(skip)}={skip}&{nameof(take)}={take}";
        var uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<PaginatedResult<Map>>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }


    public async Task<BackendApiHttpResponse<Map>> UpdateSingle(Map map)
    {
        //await Task.Yield(); // Simulate async, remove when method is implemented and add async to method
        //throw new NotImplementedException();

        // Setup for Backend communication
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(map), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{_ENDPOINT}/{map.Id.ToString()}", UriKind.Relative)
            });
    }
}