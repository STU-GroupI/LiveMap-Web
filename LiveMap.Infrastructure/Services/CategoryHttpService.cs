using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using System.Text;
using System.Text.Json;

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
    public async Task<BackendApiHttpResponse<Category>> CreateSingle(Category category)
    {
        var uri = new Uri($"{_ENDPOINT}", UriKind.Relative);
        return await _backendApiService.SendRequest<Category>(new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = uri,
            Content = new StringContent(JsonSerializer.Serialize(new { CategoryName = category.CategoryName}), Encoding.UTF8, "application/json")
        });
    }
    public async Task<BackendApiHttpResponse> Delete(Category category)
    {
        var uri = new Uri($"{_ENDPOINT}/{category.CategoryName}", UriKind.Relative);
        return await _backendApiService.SendRequest(new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = uri
        });
    }

    public async Task<BackendApiHttpResponse<Category>> UpdateSingle(Category oldVal, Category newVal)
    {
        var uri = new Uri($"{_ENDPOINT}/{oldVal.CategoryName}", UriKind.Relative);
        return await _backendApiService.SendRequest<Category>(new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = uri,
            Content = new StringContent(JsonSerializer.Serialize(new { categoryName = newVal.CategoryName }), Encoding.UTF8, "application/json")
        });
    }
}
