using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.PoiTests;

public class StubPointOfInterestRepository : IPointOfInterestRepository
{
    public readonly List<PointOfInterest> pois;
    public readonly List<Map> maps;

    public StubPointOfInterestRepository(List<PointOfInterest> pois)
    {
        this.pois = pois;
        maps = new List<Map>();
    }
    public StubPointOfInterestRepository(List<Map> maps)
    {
        this.pois = maps
            .SelectMany(m => m.PointOfInterests ?? [])
            .ToList();
        
        this.maps = maps;
    }

    public Task<PointOfInterest> CreatePointOfInterest(PointOfInterest pointOfInterest)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<PointOfInterest>> GetMultiple(Guid mapId, int? skip, int? take)
    {
        var pois = maps.Where(m => m.Id == mapId)
            .FirstOrDefault()?
            .PointOfInterests;

        if (pois is null)
        {
            return Task.FromResult<ICollection<PointOfInterest>>(new List<PointOfInterest>());
        }

        if (skip is int skipValue)
        {
            pois = pois.Skip(skipValue).ToList();
        }

        if (take is int takeValue)
        {
            pois = pois.Take(takeValue).ToList();
        }

        return Task.FromResult(pois);
    }

    public Task<PointOfInterest?> GetSingle(Guid id)
    {
        PointOfInterest? poi = pois.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(poi);
    }
}
