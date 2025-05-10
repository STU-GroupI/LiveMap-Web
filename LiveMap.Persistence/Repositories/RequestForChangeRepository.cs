using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LiveMap.Persistence.Repositories;

public class RequestForChangeRepository : IRequestForChangeRepository
{
    private readonly LiveMapContext _context;

    public RequestForChangeRepository(LiveMapContext context)
    {
        _context = context;
    }
    public async Task<RequestForChange> CreateAsync(RequestForChange requestForChange)
    {
        var rfc = requestForChange.ToSqlRequestForChange();

        rfc.SubmittedOn = DateTime.UtcNow;
        rfc.ApprovalStatus = ApprovalStatus.PENDING;

        var result = await _context.RequestsForChange.AddAsync(rfc);
        await _context.SaveChangesAsync();

        return result.Entity.ToDomainRequestForChange();
    }

    public async Task<RequestForChange?> UpdateAsync(RequestForChange requestForChange)
    {
        var existingRfc = await _context.RequestsForChange.FirstOrDefaultAsync(r => r.Id == requestForChange.Id);
        
        if (existingRfc == null)
        {
            return null;
        }
        
        existingRfc.ApprovalStatus = requestForChange.ApprovalStatus;
        existingRfc.ApprovedOn = requestForChange.ApprovedOn;
        existingRfc.Message = requestForChange.Message;

        try
        {
            _context.RequestsForChange.Update(existingRfc);
            await _context.SaveChangesAsync();

            return existingRfc.ToDomainRequestForChange();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<RequestForChange?> GetSingle(Guid id)
    {
        SqlRequestForChange? requestForChange = await _context.RequestsForChange
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync();

        return requestForChange?.ToDomainRequestForChange() ?? null;
    }

    public async Task<PaginatedResult<RequestForChange>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending, bool? IsPending)
    {
        if (take != null && take == 0)
        {
            take = 1;
        }

        IQueryable<SqlRequestForChange>? query = null;
        try
        {
            query = _context.RequestsForChange
                .Include(rfc => rfc.Poi)
                .Include(rfc => rfc.SuggestedPoi)
                .Where(rfc => parkId == (rfc.PoiId == null ? rfc.SuggestedPoi!.MapId : rfc.Poi!.MapId))
                .AsQueryable();
        } 
        catch (Exception)
        {
            throw new Exception($"If you ever see this being called, some of your RFC data is corrupt");
        }

        int totalCount = await query.CountAsync();

        if(IsPending is bool isPendingValue)
        {
            query = isPendingValue switch
            {
                true => query.Where(rfc => rfc.ApprovalStatus == ApprovalStatus.PENDING),
                false => query.Where(rfc => rfc.ApprovalStatus != ApprovalStatus.PENDING)
            };
        }

        {
            if (ascending is bool fromValue)
            {
                query = fromValue
                    ? query.OrderBy(rfc => rfc.SubmittedOn)
                    : query.OrderByDescending(rfc => rfc.SubmittedOn);
            }
            else
            {
                query = query.OrderBy(rfc => rfc.SubmittedOn);
            }
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
            return new();
        }

        return new([.. result.Select(rfc => rfc.ToDomainRequestForChange())], take, skip, totalCount);
    }

    public async Task<RequestForChange?> UpdateWithoutCommitAsync(RequestForChange requestForChange)
    {
        var existingRfc = await _context.RequestsForChange.FirstOrDefaultAsync(r => r.Id == requestForChange.Id);

        if (existingRfc == null)
        {
            return null;
        }

        existingRfc.ApprovalStatus = requestForChange.ApprovalStatus;
        existingRfc.ApprovedOn = requestForChange.ApprovedOn;
        existingRfc.Message = requestForChange.Message;

        var result = _context.RequestsForChange.Update(existingRfc);

        return result.Entity.ToDomainRequestForChange();
    }
}