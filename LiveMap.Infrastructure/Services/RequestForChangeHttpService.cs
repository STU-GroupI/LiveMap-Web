using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System.Text;
using System.Text.Json;

namespace LiveMap.Infrastructure.Services;

public class RequestForChangeHttpService : IRequestForChangeService
{
    private const string _ENDPOINT = "rfc";
    private readonly IBackendApiHttpService _backendApiService;

    public RequestForChangeHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<RequestForChange>> Get(Guid id)
    {
        var uri = new Uri($"{_ENDPOINT}/{id.ToString()}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<RequestForChange>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }

    public async Task<BackendApiHttpResponse<PaginatedResult<RequestForChange>>> GetMultiple(Guid? mapId, int? skip, int? take, bool? ascending, bool? isPending)
    {
        var query = $"{nameof(mapId)}={mapId.ToString()}&{nameof(skip)}={skip}&{nameof(take)}={take}&{nameof(ascending)}={ascending}&{nameof(isPending)}={isPending}";
        var uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<PaginatedResult<RequestForChange>>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }

    public async Task<BackendApiHttpResponse> ApproveRequestForChange(RequestForChange rfc, PointOfInterest poi)
    {
        return await _backendApiService
            .SendRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_ENDPOINT}/{rfc.Id}/approve", UriKind.Relative),
                Content = new StringContent(JsonSerializer.Serialize(new { Rfc = rfc, Poi = poi }), Encoding.UTF8, "application/json")
            });
    }

    public async Task<BackendApiHttpResponse> RejectRequestForChange(Guid rfcId)
    {
        return await _backendApiService
            .SendRequest(new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_ENDPOINT}/{rfcId.ToString()}/reject", UriKind.Relative)
            });
    }
}
