using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Domain.Models;
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

    public async Task<ICollection<RequestForChange>> GetMultiple(int? skip, int? take)
    {
        var query = _context.RequestsForChange.AsQueryable();

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

        return result.Select(rfc => rfc.ToDomainRequestForChange())
            .ToList();
    }

    public async Task<RequestForChange?> GetSingle(Guid id)
    {
        SqlRequestForChange? requestForChange = await _context.RequestsForChange
            .Include(rfc => rfc.Id)
            .Include(rfc => rfc.ApprovalStatus)
            .Include(rfc => rfc.SubmittedOn)
            .Include(rfc => rfc.Message)
            .Include(rfc => rfc.StatusProp)
            .Where(rfc => rfc.Id == id)
            .FirstOrDefaultAsync();

        if(requestForChange is null)
        {
            return null;
        }

        return requestForChange.ToDomainRequestForChange();
    }
}