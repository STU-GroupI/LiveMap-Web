using LiveMap.Application.Infrastructure.Services;
using LiveMap.Infrastructure.Extensions;

namespace LiveMap.Infrastructure.Services;

public class ImageServerHttpService(IHttpClientFactory httpClientFactory)
    : ExternalHttpServiceBase(httpClientFactory.CreateImageClient(), TimeSpan.FromMinutes(100)), IImageServerHttpService;
