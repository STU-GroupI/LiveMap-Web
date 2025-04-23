using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NetTopologySuite.Geometries.Utilities.GeometryMapper;

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

    public async Task<BackendApiHttpResponse<PaginatedResult<RequestForChange>>> GetMultiple(Guid? mapId, int? skip, int? take, bool? ascending)
    {
        var query = $"{nameof(mapId)}={mapId.ToString()}&{nameof(skip)}={skip}&{nameof(take)}={take}&{nameof(ascending)}={ascending}";
        var uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<PaginatedResult<RequestForChange>>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }
}
