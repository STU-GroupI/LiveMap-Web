using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services;
public class PointOfInterestHttpService : IPointOfInterestService
{
    private readonly IBackendApiHttpService _backendApiService;
    // TODO: God forbid we do this. Add this to a config file or so help you god I will go absolute apeshit
    private const string _ENDPOINT = "poi";

    public PointOfInterestHttpService(IBackendApiHttpService backendApiService)
    {
        _backendApiService = backendApiService;
    }

    public Task<BackendApiHttpResponse<PointOfInterest>> GetSingle(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiHttpResponse<PointOfInterest>> GetPaged(string mapId, int? from, int? to)
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
    public Task<BackendApiHttpResponse> Delete(PointOfInterest poi)
    {
        throw new NotImplementedException();
    }
}