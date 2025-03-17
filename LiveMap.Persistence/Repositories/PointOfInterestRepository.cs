using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extentions;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence.Repositories;

public class PointOfInterestRepository : IPointOfInterestRepository
{
    private readonly LiveMapContext _context;

    public PointOfInterestRepository(LiveMapContext context)
    {
        _context = context;
    }

    public Task<PointOfInterest> GetPaged(Guid mapId, int from, int amount)
    {
        throw new NotImplementedException();
    }

    public async Task<PointOfInterest?> GetSingle(Guid id)
    {
        SqlPointOfInterest? pointOfInterest = await _context.PointsOfInterest
            .Include(poi => poi.Category)
            .Include(poi => poi.Status)
            .Include(poi => poi.Map)
            .Where(poi => poi.Id == id)
            .FirstOrDefaultAsync();

        if(pointOfInterest is null)
        {
            return null;
        }


        return pointOfInterest.ToPointOfInterest();
    }
}