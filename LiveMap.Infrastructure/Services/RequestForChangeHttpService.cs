using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text.Json;
using System.Text;

namespace LiveMap.Infrastructure.Services;
public class RequestForChangeHttpService : IRequestForChangeService
{
    private const string _ENDPOINT = "rfc";
    private readonly IBackendApiHttpService _backendApiHttpService;

    public RequestForChangeHttpService(IBackendApiHttpService backendApiHttpService)
    {
        _backendApiHttpService = backendApiHttpService;
    }

    public async Task<BackendApiHttpResponse<RequestForChange>> Approve(RequestForChange rfc)
    {
        rfc.ApprovalStatus = ApprovalStatus.APPROVED;

        return await _backendApiHttpService.SendRequest<RequestForChange>(new HttpRequestMessage()
        {
            Method = HttpMethod.Patch,
            Content = new StringContent(JsonSerializer.Serialize(new { rfc.Id, rfc.ApprovalStatus }), Encoding.UTF8, "application/json"),
            RequestUri = new Uri($"{_ENDPOINT}/{rfc.Id}", UriKind.Relative),
        });
    }

    public async Task<BackendApiHttpResponse<RequestForChange>> Deny(RequestForChange rfc)
    {
        rfc.ApprovalStatus = ApprovalStatus.REJECTED;

        return await _backendApiHttpService.SendRequest<RequestForChange>(new HttpRequestMessage()
        {
            Method = HttpMethod.Patch,
            Content = new StringContent(JsonSerializer.Serialize(new { rfc.Id, rfc.ApprovalStatus }), Encoding.UTF8, "application/json"),
            RequestUri = new Uri($"{_ENDPOINT}/{rfc.Id}", UriKind.Relative),
        });
    }

    public Task<BackendApiHttpResponse<RequestForChange>> CreateSingle(RequestForChange rfc)
    {
        throw new NotImplementedException();
    }

    public async Task<BackendApiHttpResponse<RequestForChange>> Get(Guid id)
    {
        return await _backendApiHttpService.SendRequest<RequestForChange>(new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_ENDPOINT}/{id}", UriKind.Relative)
        });
    }

    public Task<BackendApiHttpResponse<RequestForChange[]>> Get(int? skip, int? take)
    {
        throw new NotImplementedException();
    }

    public Task<BackendApiHttpResponse<RequestForChange>> UpdateSingle(RequestForChange rfc)
    {
        throw new NotImplementedException();
    }

    public Task<BackendApiHttpResponse> Delete(RequestForChange rfc)
    {
        throw new NotImplementedException();
    }
}
