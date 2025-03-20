using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.PoiTests;

public class StubPointOfInterestRepository : IPointOfInterestRepository
{
    public readonly List<PointOfInterest> points;
    public readonly List<Map> maps;

    public StubPointOfInterestRepository(List<PointOfInterest> points)
    {
        this.points = points;
        maps = new List<Map>();
    }
    public StubPointOfInterestRepository(List<Map> maps)
    {
        this.points = maps
            .SelectMany(m => m.PointOfInterests ?? [])
            .ToList();
        
        this.maps = maps;
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
        PointOfInterest? poi = points.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(poi);
    }
}
