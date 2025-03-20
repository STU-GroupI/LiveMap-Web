using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Persistance;

public interface IPointOfInterestRepository
{
    public Task<Models.PointOfInterest?> GetSingle(Guid id);

    public Task<ICollection<Models.PointOfInterest>> GetMultiple(Guid mapId, int? skip, int? take);
}