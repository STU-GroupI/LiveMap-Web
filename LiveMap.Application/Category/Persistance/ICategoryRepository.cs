using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Persistance;
public interface ICategoryRepository
{
    public Task<Models.Category> Create(Models.Category category);
    public Task<Models.Category?> GetSingle(string name);

    public Task<Models.Category[]> GetMultiple(int? skip, int? take);
    public Task<bool> Update(string oldName, string newName);
    public Task<bool> Delete(string name);
}
