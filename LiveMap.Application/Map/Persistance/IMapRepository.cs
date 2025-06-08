using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;

namespace LiveMap.Application.Map.Persistance;
using Models = Domain.Models;

public interface IMapRepository
{
    public Task<Models.Map?> GetSingle(Guid id);

    public Task<Models.Map?> GetClosest(double latitude, double longitude);

    public Task<Models.Map> CreateAsync(Models.Map map);

    public Task<PaginatedResult<Models.Map>> GetMultiple(int? skip, int? take);

    public Task<Models.Map?> Update(Models.Map map);

    public Task<bool> Delete(Guid id);
}