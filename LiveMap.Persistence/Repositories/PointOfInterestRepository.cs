using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;

namespace LiveMap.Persistence.Repositories;

public class PointOfInterestRepository : IPointOfInterestRepository
{
    public Task<PointOfInterest> GetPaged(int from, int amount)
    {
        throw new NotImplementedException();
    }

    public Task<PointOfInterest> GetSingle(int id)
    {
        throw new NotImplementedException();
    }
}