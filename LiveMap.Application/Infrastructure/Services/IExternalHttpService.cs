using LiveMap.Application.Infrastructure.Models;

namespace LiveMap.Application.Infrastructure.Services;

public interface IExternalHttpService
{
    Task<ExternalHttpResponse> SendRequest(HttpRequestMessage request);
    Task<ExternalHttpResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request) where TResult : class;
}
