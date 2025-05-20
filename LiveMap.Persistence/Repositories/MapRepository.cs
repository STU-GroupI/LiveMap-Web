using LiveMap.Application.Map.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence.Repositories;

public class MapRepository : IMapRepository
{
    private readonly LiveMapContext _context;

    public MapRepository(LiveMapContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Map>> GetMultiple(int? skip, int? take)
    {
        if (take != null && take == 0)
        {
            take = 1;
        }

        var query = _context.Maps.AsQueryable();
        int totalCount = await query.CountAsync();

        if (skip is int fromValue)
        {
            query = query.Skip(fromValue);
        }

        if (take is int amountValue)
        {
            query = query.Take(amountValue);
        }

        var result = await query.ToListAsync();
        if (result is not { Count: > 0 })
        {
            return new();
        }

        return new([.. result.Select(map => map.ToDomainMap())], take, skip, totalCount);
    }

    public async Task<Map?> GetSingle(Guid id)
    {
        SqlMap? map = await _context.Maps
            .Include(map => map.PointOfInterests)
            .Where(map => map.Id == id)
            .FirstOrDefaultAsync();

        if (map is null)
        {
            return null;
        }

        return map.ToDomainMap();
    }

    public async Task<Map> CreateAsync(Map map)
    {
        var newMap = map.ToSqlMap();

        var result = await _context.Maps.AddAsync(newMap);
        await _context.SaveChangesAsync();

        return result.Entity.ToDomainMap();
    }

    public async Task<Map?> Update(Map map)
    {
        var newMap = await _context.Maps
            .Where(m => m.Id == map.Id)
            .FirstOrDefaultAsync();

        if (newMap is null)
        {
            return null;
        }
        
        newMap.Name = map.Name;
        newMap.Bounds = map.Bounds.ToPolygon();
        newMap.Border = map.Area.ToPolygon();

        if(map.ImageUrl is not null)
        {
            newMap.ImageUrl = map.ImageUrl;
        }

        await _context.SaveChangesAsync();

        return newMap.ToDomainMap();
    }
}