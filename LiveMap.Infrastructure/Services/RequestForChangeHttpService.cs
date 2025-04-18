using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Infrastructure.Services;

public class RequestForChangeHttpService : IRequestForChangeService
{
    private const string _ENDPOINT = "rfc";
    private readonly IBackendApiHttpService _backendApiService;

    public RequestForChangeHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<RequestForChange[]>> Get(Guid mapId, int? skip, int? take, bool? ascending)
    {
        var query = $"{nameof(skip)}={skip}&{nameof(take)}={take}&{nameof(ascending)}={ascending}";
        var uri = new Uri($"{_ENDPOINT}/{mapId}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<RequestForChange[]>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }
}
