using LiveMap.Application.PointOfInterest.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;
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
            .Include(poi => poi.OpeningHours)
            .Include(poi => poi.Category)
            .Where(poi => poi.MapId == mapId);

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
            return [];
        }

        return result.Select(poi => poi.ToDomainPointOfInterest())
            .ToList();
    }

    public async Task<PointOfInterest?> GetSingle(Guid id)
    {
        SqlPointOfInterest? pointOfInterest = await _context.PointsOfInterest
            .Include(poi => poi.Category)
            .Include(poi => poi.Status)
            .Include(poi => poi.Map)
            .Include(poi => poi.OpeningHours)
            .Where(poi => poi.Id == id)
            .FirstOrDefaultAsync();

        if (pointOfInterest is null)
        {
            return null;
        }


        return pointOfInterest.ToDomainPointOfInterest();
    }

    public async Task<PointOfInterest> Create(PointOfInterest pointOfInterest)
    {
        var poi = pointOfInterest.ToSqlPointOfInterest();

        var result = await _context.PointsOfInterest.AddAsync(poi);
        await _context.SaveChangesAsync();

        return result.Entity.ToDomainPointOfInterest();
    }

    public async Task<PointOfInterest?> Update(PointOfInterest pointOfInterest)
    {
        var poi = await _context.PointsOfInterest
            .Include(poi => poi.OpeningHours)
            .Where(poi => poi.Id == pointOfInterest.Id)
            .FirstOrDefaultAsync();

        if (poi is null)
        {
            return null;
        }

        poi.Title = pointOfInterest.Title;
        poi.Description = pointOfInterest.Description;
        poi.Image = pointOfInterest.Image;
        poi.CategoryName = pointOfInterest.CategoryName;
        poi.Position = pointOfInterest.Coordinate.ToSqlPoint();
        poi.IsWheelchairAccessible = pointOfInterest.IsWheelchairAccessible;

        // Track opening hours to remove
        var openingHoursToRemove = new List<SqlOpeningHours>();
        
        foreach (var openingHour in poi.OpeningHours)
        {
            var data = pointOfInterest.OpeningHours?.FirstOrDefault(oh =>
                oh.DayOfWeek == openingHour.DayOfWeek);

            if (data is null)
            {
                openingHoursToRemove.Add(openingHour);
            }
            else
            {
                openingHour.Start = data.Start;
                openingHour.End = data.End;
            }
        }

        foreach (var openingHour in openingHoursToRemove)
        {
            poi.OpeningHours.Remove(openingHour);
            _context.OpeningHours.Remove(openingHour);
        }

        // Add new opening hours directly
        foreach (var newOpeningHour in pointOfInterest.OpeningHours ?? Enumerable.Empty<OpeningHours>())
        {
            if (poi.OpeningHours.Any(oh => oh.DayOfWeek == newOpeningHour.DayOfWeek))
            {
                continue;
            }
            
            var newSqlOpeningHour = new SqlOpeningHours
            {
                Id = Guid.NewGuid(),
                DayOfWeek = newOpeningHour.DayOfWeek,
                Start = newOpeningHour.Start,
                End = newOpeningHour.End,
                PoiId = poi.Id
            };
            poi.OpeningHours.Add(newSqlOpeningHour);
            _context.OpeningHours.Add(newSqlOpeningHour);
        }

        await _context.SaveChangesAsync();

        return poi.ToDomainPointOfInterest();
    }

    public async Task<bool> DeleteSingle(Guid id)
    {
        SqlPointOfInterest? pointOfInterest = await _context.PointsOfInterest
            .Where(poi => poi.Id == id)
            .FirstOrDefaultAsync();

        if (pointOfInterest is null)
        {
            return false;
        }

        List<SqlRequestForChange> requestForChanges = await _context.RequestsForChange
            .Where(rfc => rfc.PoiId == id)
            .ToListAsync();

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            foreach (var requestForChange in requestForChanges)
            {
                _context.RequestsForChange.Remove(requestForChange);
            }
            _context.PointsOfInterest.Remove(pointOfInterest);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            // Thomas: 
            // Currently, we can do it this way. But if an exception is thrown within the scope
            // of an active transaction, it is automagically rolled back. Thats why you don't
            // see me do it in the cascading delete of the suggested POI's
            
            await transaction.RollbackAsync();
            throw;
        }
    }
}