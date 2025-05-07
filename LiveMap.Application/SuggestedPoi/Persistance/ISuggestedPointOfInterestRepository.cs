using LiveMap.Domain.Models;

namespace LiveMap.Application.SuggestedPoi.Persistanc;
public interface ISuggestedPointOfInterestRepository
{
    public Task<SuggestedPointOfInterest> CreateSuggestedPointOfInterest(SuggestedPointOfInterest suggestedPoi);
    public Task<SuggestedPointOfInterest?> ReadSingle(Guid id);
    public Task DeleteWithoutCommitAsync(Guid id);
}
