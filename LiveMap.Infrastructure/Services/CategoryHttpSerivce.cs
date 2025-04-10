using LiveMap.Application.Infrastructure.Models;
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;

namespace LiveMap.Infrastructure.Services;

public class CategoryHttpSerivce : ICategoryService
{
    public Task<BackendApiHttpResponse<Category>> GetAll()
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiHttpResponse<Category>> GetSingle(Guid id)
    {
        throw new NotImplementedException();
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
