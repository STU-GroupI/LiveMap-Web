using LiveMap.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Application.SuggestedPoi.Persistanc;
public interface ISuggestedPointOfInterestRepository
{
    public Task<SuggestedPointOfInterest> CreateSuggestedPointOfInterest(SuggestedPointOfInterest suggestedPoi);
    public Task<ICollection<SuggestedPointOfInterest>> GetMultiple(Guid parkId, int? skip, int? take);
}
