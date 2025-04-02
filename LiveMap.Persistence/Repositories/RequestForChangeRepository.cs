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
    public async Task<RequestForChange> CreateAsync(RequestForChange requestForChange)
    {
        var rfc = requestForChange.ToSqlRequestForChange();

        rfc.SubmittedOn = DateTime.UtcNow;
        rfc.StatusProp = new ApprovalStatus()
        {
            Status = "Pending"
        };

        _context.Entry(rfc.StatusProp).State = EntityState.Unchanged;
        var result = await _context.RequestsForChange.AddAsync(rfc);
        await _context.SaveChangesAsync();
        
        return result.Entity.ToDomainRequestForChange();
    }
}