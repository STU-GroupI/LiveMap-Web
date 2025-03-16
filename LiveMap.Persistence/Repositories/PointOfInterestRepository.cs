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
    private readonly LiveMapContext _context;

    public PointOfInterestRepository(LiveMapContext context)
    {
        _context = context;
    }

    public Task<PointOfInterest> GetPaged(int parkId, int from, int amount)
    {
        throw new NotImplementedException();
    }

    public Task<PointOfInterest> GetSingle(int id)
    {
        PointOfInterest? pointOfInterest = null;

        if (pointOfInterest is null)
        {
            return Task.FromException<PointOfInterest>(new ArgumentNullException(nameof(id)));
        }

        return Task.FromResult(pointOfInterest);
    }
}