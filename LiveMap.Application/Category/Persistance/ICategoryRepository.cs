using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Persistance
{
public interface ICategoryRepository
{
    public Task<Models.Category?> GetSingle(string name);
    public Task<Models.Category[]> GetMultiple(int? skip, int? take);
    public Task<bool> Delete(Models.Category category);
}
