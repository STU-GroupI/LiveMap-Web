using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;

namespace LiveMap.Infrastructure.Services;

public class CategoryHttpService : ICategoryService
{
    private const string _ENDPOINT = "category";
    private readonly IBackendApiHttpService _backendApiService;

    public CategoryHttpService(IBackendApiHttpService backendApiService)
    {
        _backendApiService = backendApiService;
    }

    public async Task<BackendApiHttpResponse<Category>> Get(string name)
    {
        var uri = new Uri($"{_ENDPOINT}/{name}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<Category>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }

    public async Task<BackendApiHttpResponse<Category[]>> Get(int? skip, int? take)
    {
        var query = $"{nameof(skip)}={skip}&{nameof(take)}={take}";
        var uri = new Uri($"{_ENDPOINT}?{query}", UriKind.Relative);

        return await _backendApiService
            .SendRequest<Category[]>(new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            });
    }
    public Task<BackendApiHttpResponse<Category>> CreateSingle(Category poi)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiHttpResponse<Category>> UpdateSingle(Category poi)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiHttpResponse> Delete(Category poi)
    {
        throw new NotImplementedException();
    }
}
