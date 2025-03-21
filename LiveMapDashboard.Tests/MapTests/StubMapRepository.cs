using LiveMap.Application.Map.Persistance;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.MapTests;

public class StubMapRepository : IMapRepository
{
    public readonly List<Map> maps;

    public StubMapRepository(List<Map> maps)
    {
        this.maps = maps;
    }

    public Task<ICollection<Map>> GetMultiple(int? skip, int? take)
    {
        if (maps is not { Count: > 0 })
        {
            return Task.FromResult<ICollection<Map>>(new List<Map>());
        }
        var data = new List<Map>(maps);

        if (skip is int skipValue)
        {
            data = data.Skip(skipValue).ToList();
        }

        if (take is int takeValue)
        {
            data = data.Take(takeValue).ToList();
        }

        return Task.FromResult<ICollection<Map>>(data);
    }

    public Task<Map?> GetSingle(Guid id)
    {
        Map? map = maps.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(map);
    }
}
