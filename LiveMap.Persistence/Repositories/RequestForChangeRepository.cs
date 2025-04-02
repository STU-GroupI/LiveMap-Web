using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Persistence.DbModels;
using LiveMap.Persistence.Extensions;

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
        var data = new SqlRequestForChange()
        {
            Id = requestForChange.Id,
            ApprovalStatus = requestForChange.ApprovalStatus,
            ApprovedOn = requestForChange.ApprovedOn,
            StatusProp = new ApprovalStatus()
            {
                Status = "Pending"
            },
            SubmittedOn = requestForChange.SubmittedOn,
            PoiId = requestForChange.PoiId,
            Message = requestForChange.Message,
        };
        var result = _context.RequestsForChange.Add(data);
        await _context.SaveChangesAsync();
        return result.Entity.ToDomainRequestForChange();
    }
}