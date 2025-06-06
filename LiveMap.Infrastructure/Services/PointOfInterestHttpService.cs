using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LiveMap.Infrastructure.Services;
public class PointOfInterestHttpService : IPointOfInterestService
{
    private readonly IBackendApiHttpService _backendApiService;
    // TODO: Turn this into configuration over contract
    private const string _ENDPOINT = "poi";

    public PointOfInterestHttpService(IBackendApiHttpService backendApiService)
    {
        _backendApiService = backendApiService;
    }

    public async Task<ExternalHttpResponse<PointOfInterest>> Get(Guid id)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse<PointOfInterest[]>> Get(string mapId, int? skip, int? take)
    {
        string query = $"{nameof(mapId)}={mapId}&{nameof(skip)}={skip}&{nameof(take)}={take}";
        Uri uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);
        return await _backendApiService
            .SendRequest<PointOfInterest[]>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }
    public async Task<ExternalHttpResponse<PointOfInterest>> CreateSingle(PointOfInterest poi)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(poi), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_ENDPOINT, UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse<PointOfInterest>> UpdateSingle(PointOfInterest poi)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(poi), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{_ENDPOINT}/{poi.Id.ToString()}", UriKind.Relative)
            });
    }

    public async Task<ExternalHttpResponse> Delete(Guid id)
    {
        return await _backendApiService
            .SendRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative)
            });
    }
}