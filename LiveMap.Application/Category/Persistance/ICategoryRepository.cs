using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.Category.Persistance;
public interface ICategoryRepository
{
    public Task<bool> Delete(Models.Category category);
}
