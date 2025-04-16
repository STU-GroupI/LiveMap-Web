using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Persistance;
public interface ICategoryRepository
{
    public Task<Models.Category> Create(Models.Category category);
    public Task<Models.Category?> GetSingle(string name);

    public Task<ICollection<Models.Category>> GetMultiple(string name, int? skip, int? take);
    public Task<bool> Update(Models.Category category);
    public Task<bool> Delete(Models.Category category);
}
