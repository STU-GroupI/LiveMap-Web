using LiveMap.Domain.Models;

namespace LiveMapDashboard.Web.Services.Communication;

public interface ICategoryService
{
    Task<BackendApiResponse<Category>> GetSingle(Guid id);
    Task<BackendApiResponse<Category>> GetAll();
    Task<BackendApiResponse<Category>> CreateSingle(Category poi);
    Task<BackendApiResponse<Category>> UpdateSingle(Category poi);
    Task<BackendApiResponse> Delete(Category poi);
}

public class CategoryHttpSerivce : ICategoryService
{
    public Task<BackendApiResponse<Category>> GetAll()
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse<Category>> GetSingle(Guid id)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse<Category>> CreateSingle(Category poi)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse<Category>> UpdateSingle(Category poi)
    {
        throw new NotImplementedException();
    }
    public Task<BackendApiResponse> Delete(Category poi)
    {
        throw new NotImplementedException();
    }
}
