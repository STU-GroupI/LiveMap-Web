using LiveMap.Application.Infrastructure.Models;

namespace LiveMap.Application.Infrastructure.Services;
public interface IBackendApiHttpService
{
    Task<BackendApiHttpResponse> SendRequest(HttpRequestMessage request);
    Task<BackendApiHttpResponse<TResult>> SendRequest<TResult>(HttpRequestMessage request) where TResult : class;
}
