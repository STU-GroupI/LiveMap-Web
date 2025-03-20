using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Tests.PoiTests;

public class StubPointOfInterestRepository : IPointOfInterestRepository
{
    public readonly List<PointOfInterest> points;

    public StubPointOfInterestRepository(List<PointOfInterest> points)
    {
        this.points = points;
    }

    public Task<ICollection<PointOfInterest>> GetMultiple(Guid parkId, int? from, int? amount)
    {
        throw new NotImplementedException();
    }

    public Task<PointOfInterest?> GetSingle(Guid id)
    {
        PointOfInterest? poi = points.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(poi);
    }
}
