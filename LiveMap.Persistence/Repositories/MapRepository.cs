using Bogus.DataSets;
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

        return new(result.Select(map => map.ToDomainMap()).ToList(), take, skip, totalCount);
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
        var foundMap = await _context.Maps
            .Where(m => m.Id == map.Id)
            .FirstOrDefaultAsync();

        if (foundMap is null)
        {
            return null;
        }

        foundMap.Name = map.Name;
        foundMap.Bounds = map.Bounds.ToPolygon();
        foundMap.Border = map.Area.ToPolygon();
        foundMap.ImageUrl = map.ImageUrl ?? null;

        await _context.SaveChangesAsync();

        return foundMap.ToDomainMap();
    }

    public async Task<bool> Delete(Guid id)
    {
        SqlMap? map = await _context.Maps
            .Where(map => map.Id == id)
            .FirstOrDefaultAsync();

        if (map is null)
        {
            return false;
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var suggestedPoisToDelete = await _context.SuggestedPointsOfInterest
                .Where(sugPoi => sugPoi.MapId == id)
                .ToListAsync();

            var poisToDelete = await _context.PointsOfInterest
                .Where(poi => poi.MapId == id)
                .ToListAsync();

            var requestsForChangeToDelete = await _context.RequestsForChange
                .Where(rfc => (rfc.Poi != null && poisToDelete.Contains(rfc.Poi)) ||
                              (rfc.SuggestedPoi != null && suggestedPoisToDelete.Contains(rfc.SuggestedPoi)))
                .ToListAsync();

            foreach (var request in requestsForChangeToDelete)
            {
                request.SuggestedPoi = null;
                request.Poi = null;
            }

            _context.SuggestedPointsOfInterest.RemoveRange(suggestedPoisToDelete);
            _context.RequestsForChange.RemoveRange(requestsForChangeToDelete);
            _context.PointsOfInterest.RemoveRange(poisToDelete);
            _context.Maps.Remove(map);
            
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}