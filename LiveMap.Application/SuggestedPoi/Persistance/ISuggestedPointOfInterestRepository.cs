using LiveMap.Domain.Models;

namespace LiveMap.Application.SuggestedPoi.Persistanc;
public interface ISuggestedPointOfInterestRepository
{
    public Task<SuggestedPointOfInterest> CreateSuggestedPointOfInterest(SuggestedPointOfInterest suggestedPoi);
}
