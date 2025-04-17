using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text;
using System.Text.Json;

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

    public Task<BackendApiHttpResponse<PointOfInterest>> Get(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiHttpResponse<PointOfInterest[]>> Get(string mapId, int? skip, int? take)
    {
        throw new NotImplementedException();
    }
    public async Task<BackendApiHttpResponse<PointOfInterest>> CreateSingle(PointOfInterest poi)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(poi), Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_ENDPOINT, UriKind.Relative)
            });
    }
    public Task<BackendApiHttpResponse<PointOfInterest>> UpdateSingle(PointOfInterest poi)
    {
        throw new NotImplementedException();
    }
    public async Task<BackendApiHttpResponse<PointOfInterest>> Delete(Guid id)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_ENDPOINT}/{id}", UriKind.Relative)
            });
    }
}