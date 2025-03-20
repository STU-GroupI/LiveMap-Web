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

    public async Task<ICollection<PointOfInterest>> GetMultiple(Guid mapId, int? skip, int? take)
    {
        var query = _context.PointsOfInterest
            .Where(poi => poi.MapId == mapId);

        if (skip is int fromValue)
        {
            query = query.Skip(fromValue);
        }

        if(take is int amountValue)
        {
            query = query.Take(amountValue);
        }
        
        var result = await query.ToListAsync();

        if (result is not { Count: > 0 }) 
        {
            return [];
        }

        return result.Select(poi => poi.ToPointOfInterest())
            .ToList();
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