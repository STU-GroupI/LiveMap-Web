using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Persistance;

public interface IPointOfInterestRepository
{
    public Task<Models.PointOfInterest?> GetSingle(Guid id);

    public Task<ICollection<Models.PointOfInterest>> GetMultiple(Guid mapId, int? skip, int? take);

    public Task<Models.PointOfInterest> CreatePointOfInterest(Models.PointOfInterest pointOfInterest);

    public Task<Models.PointOfInterest?> DeleteSingle(Guid id);
}