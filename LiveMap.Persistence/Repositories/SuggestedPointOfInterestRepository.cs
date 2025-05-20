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
        SqlRequestForChange rfc = new SqlRequestForChange
        {
            SubmittedOn = DateTime.UtcNow,
            ApprovalStatus = ApprovalStatus.PENDING,
            Id = default,
            ApprovalStatusProp = new ApprovalStatus { Status = ApprovalStatus.PENDING },
        };

        var sqlData = suggestedPoi.ToSqlSuggestedPointOfInterest();
        sqlData.RFC = rfc;

        if (sqlData.Map is not null) _context.Entry(sqlData.Map).State = EntityState.Unchanged;
        if (sqlData.Category is not null) _context.Entry(sqlData.Category).State = EntityState.Unchanged;

        _context.Entry(sqlData.RFC.ApprovalStatusProp).State = EntityState.Unchanged;
        var entity = await _context.SuggestedPointsOfInterest.AddAsync(sqlData);
        await _context.SaveChangesAsync();

        var response = entity.Entity.ToDomainSuggestedPointOfInterest();
        response.RFC = rfc.ToDomainRequestForChange();

        return response;
    }

    public async Task DeleteWithoutCommitAsync(Guid id)
    {
        var poi = await _context.SuggestedPointsOfInterest.FindAsync(id);
        
        if (poi is null) 
            return;

        _context.SuggestedPointsOfInterest.Remove(poi);
    }

    public async Task<SuggestedPointOfInterest?> ReadSingle(Guid id)
    {
        return (await _context.SuggestedPointsOfInterest.FirstOrDefaultAsync(x => x.Id == id))
            ?.ToDomainSuggestedPointOfInterest() ?? null;
    }
}
