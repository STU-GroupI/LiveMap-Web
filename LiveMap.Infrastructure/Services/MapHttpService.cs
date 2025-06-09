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

    public async Task<ExternalHttpResponse<Map>> CreateSingle(Map map)
    {
        // Setup for Backend communication
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(map), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_ENDPOINT, UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse<Map>> Get(Guid id)
    {
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse<PaginatedResult<Map>>> Get(int? skip, int? take)
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


    public async Task<ExternalHttpResponse<Map>> UpdateSingle(Map map)
    {
        // Setup for Backend communication
        return await _backendApiService
            .SendRequest<Map>(new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(map), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{_ENDPOINT}/{map.Id.ToString()}", UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse> Delete(Guid id)
    {
        return await _backendApiService
            .SendRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            }); ;
    }
}