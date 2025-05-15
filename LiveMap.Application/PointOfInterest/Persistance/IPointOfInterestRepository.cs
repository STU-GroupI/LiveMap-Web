using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Persistance;

public interface IPointOfInterestRepository
{
    public Task<Models.PointOfInterest?> GetSingle(Guid id);
    public Task<ICollection<Models.PointOfInterest>> GetMultiple(Guid mapId, int? skip, int? take);
    public Task<Models.PointOfInterest> Create(Models.PointOfInterest pointOfInterest);
    public Task<Models.PointOfInterest?> Update(Models.PointOfInterest pointOfInterest);
    public Task<bool> DeleteSingle(Guid id);

    public Task<Models.PointOfInterest> CreateWithoutCommitAsync(Models.PointOfInterest pointOfInterest);
    public Task<Models.PointOfInterest?> UpdateWithoutCommitAsync(Models.PointOfInterest pointOfInterest);
}