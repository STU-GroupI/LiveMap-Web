using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;

namespace LiveMap.Application.Map.Persistance;
public interface IMapRepository
{
    public Task<Models.Map?> GetSingle(Guid id);

    public Task<ICollection<Models.Map>> GetMultiple(int? skip, int? take);

    public Task<Models.Map> CreateAsync(Models.Map map);

    public Task<PaginatedResult<Domain.Models.Map>> GetMultiple(int? skip, int? take);

    public Task<Models.Map?> Update(Models.Map map);
}

