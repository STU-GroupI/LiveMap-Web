using LiveMap.Application.Infrastructure.Services;
using LiveMap.Infrastructure.Extensions;

namespace LiveMap.Infrastructure.Services;

public class BackendApiHttpService(IHttpClientFactory httpClientFactory) 
    : ExternalHttpServiceBase(httpClientFactory.CreateBackendClient(), TimeSpan.FromMinutes(100)), IBackendApiHttpService;