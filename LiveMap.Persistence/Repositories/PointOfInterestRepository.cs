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
        var poi = await _context.PointsOfInterest.FindAsync(pointOfInterest.Id);

        if (poi is null)
        {
            return null;
        }

        poi.Title = pointOfInterest.Title;
        poi.Description = pointOfInterest.Description;
        poi.CategoryName = pointOfInterest.CategoryName;
        poi.Position = pointOfInterest.Coordinate.ToSqlPoint();
        poi.IsWheelchairAccessible = pointOfInterest.IsWheelchairAccessible;

        foreach (var openingHour in poi.OpeningHours)
        {
            var data = pointOfInterest.OpeningHours.FirstOrDefault(oh =>
                oh.DayOfWeek == openingHour.DayOfWeek);

            if (data is null)
            {
                poi.OpeningHours.Add(openingHour);
                continue;
            }

            data.Start = openingHour.Start;
            data.End = openingHour.End;
        }

        foreach (var openingHour in poi.OpeningHours.Where(oh => !pointOfInterest.OpeningHours
                .Any(oh2 => oh2.DayOfWeek == oh.DayOfWeek)))
        {
            poi.OpeningHours.Remove(openingHour);
            _context.OpeningHours.Remove(openingHour);
        }

        await _context.SaveChangesAsync();

        return poi.ToDomainPointOfInterest();
    }
}