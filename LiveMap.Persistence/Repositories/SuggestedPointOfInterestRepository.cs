using LiveMap.Application.SuggestedPoi.Persistanc;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LiveMap.Persistence.Repositories;
public class SuggestedPointOfInterestRepository : ISuggestedPointOfInterestRepository
{
    private readonly LiveMapContext _context;

    public SuggestedPointOfInterestRepository(LiveMapContext context)
    {
        _context = context;
    }

    public async Task<SuggestedPointOfInterest> CreateSuggestedPointOfInterest(SuggestedPointOfInterest suggestedPoi)
    {
        SqlRequestForChange rfc = new SqlRequestForChange()
        {
            SubmittedOn = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.PENDING,
        };

        var sqlData = suggestedPoi.ToSqlSuggestedPointOfInterest();
        sqlData.RFC = rfc;

        if (sqlData.Map is not null) _context.Entry(sqlData.Map).State = EntityState.Unchanged;
        if (sqlData.Category is not null) _context.Entry(sqlData.Category).State = EntityState.Unchanged;


        var entity = await _context.SuggestedPointsOfInterest.AddAsync(sqlData);
        await _context.SaveChangesAsync();

        var response = entity.Entity.ToDomainSuggestedPointOfInterest();
        response.RFC = rfc.ToDomainRequestForChange();

        return response;
    }

    public async Task<ICollection<SuggestedPointOfInterest>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending)
    {
        var query = _context.SuggestedPointsOfInterest
            .Where(spoi => spoi.MapId == parkId)
            .Include(spoi => spoi.RFC)
            .AsQueryable();

        {
            if (ascending is bool fromValue)
            {
                query = fromValue
                    ? query.OrderBy(spoi => spoi.RFC.SubmittedOn)
                    : query.OrderByDescending(spoi => spoi.RFC.SubmittedOn);
            }
            query = query.OrderBy(spoi => spoi.RFC.SubmittedOn);
        }

        {
            if (skip is int fromValue)
            {
                query = query.Skip(fromValue);
            }
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

        return result.Select(spoi => spoi.ToDomainSuggestedPointOfInterest()).ToList();
    }
}
