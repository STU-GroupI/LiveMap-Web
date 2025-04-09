using LiveMap.Domain.Models;
using System.Text.Json;

namespace LiveMapDashboard.Web.Services.Communication;

public interface IPointOfInterestService
{
    Task<BackendApiResponse<PointOfInterest>> GetSingle(Guid id);
    Task<BackendApiResponse<PointOfInterest>> GetPaged(string mapId, int? from, int? to);
    Task<BackendApiResponse<PointOfInterest>> CreateSingle(PointOfInterest poi);
    Task<BackendApiResponse<PointOfInterest>> UpdateSingle(PointOfInterest poi);
    Task<BackendApiResponse> Delete(PointOfInterest poi);
}
public class PointOfInterestHttpService : IPointOfInterestService
{
    private readonly IBackendApiService _backendApiService;
    // TODO: God forbid we do this. Add this to a config file or so help you god I will go absolute apeshit
    private const string _endpoint = "poi";

    public PointOfInterestHttpService(IBackendApiService backendApiService)
    {
        _backendApiService = backendApiService.ConfigureClient(client =>
        {
            client.BaseAddress = new Uri($"{client.BaseAddress!.ToString()}/{_endpoint}");
        });
    }

    public Task<BackendApiResponse<PointOfInterest>> GetSingle(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse<PointOfInterest>> GetPaged(string mapId, int? from, int? to)
    {
        throw new NotImplementedException();
    }
    public async Task<BackendApiResponse<PointOfInterest>> CreateSingle(PointOfInterest poi)
    {
        return await _backendApiService
            .SendRequest<PointOfInterest>(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(poi)),
            });
    }
    public Task<BackendApiResponse<PointOfInterest>> UpdateSingle(PointOfInterest poi)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse> Delete(PointOfInterest poi)
    {
        throw new NotImplementedException();
    }
}