using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Infrastructure.Services;

public class SuggestedPointOfInterestHttpService : ISuggestedPointOfInterestService
{
    private const string _ENDPOINT = "rfc/poisuggestion";
    private readonly IBackendApiHttpService _backendApiService;

    public SuggestedPointOfInterestHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<SuggestedPointOfInterest[]>> Get(Guid mapId, int? skip, int? take, bool? ascending)
    {
        var query = $"{nameof(skip)}={skip}&{nameof(take)}={take}&{nameof(ascending)}={ascending}";
        var uri = new Uri($"{_ENDPOINT}/{mapId}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<SuggestedPointOfInterest[]>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }
}
