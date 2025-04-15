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
        rfc.ApprovalStatus = ApprovalStatus.PENDING;

        var result = await _context.RequestsForChange.AddAsync(rfc);
        await _context.SaveChangesAsync();
        
        return result.Entity.ToDomainRequestForChange();
    }

    public async Task<RequestForChange> UpdateAsync(RequestForChange requestForChange)
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
}