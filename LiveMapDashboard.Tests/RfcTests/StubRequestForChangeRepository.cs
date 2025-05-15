using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;

namespace LiveMapDashboard.Tests.RfcTests;
public class StubRequestForChangeRepository : IRequestForChangeRepository
{
    public List<RequestForChange> RequestsForChange { get; }
    public List<Guid> KnownPois { get; }

    public StubRequestForChangeRepository(List<RequestForChange> requestsForChangte, List<Guid> knownPois)
    {
        RequestsForChange = requestsForChangte;
        KnownPois = knownPois;
    }
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange)
    {
        return Task.FromResult(new RequestForChange()
        {
            Id = requestForChange.Id,
            Message = requestForChange.Message,
            ApprovalStatus = ApprovalStatus.PENDING,
            PoiId = requestForChange.PoiId,
            SuggestedPoiId = requestForChange.SuggestedPoiId,
            SubmittedOn = DateTime.UtcNow,
        });
    }

    public Task<RequestForChange?> UpdateAsync(RequestForChange requestForChange)
    {
        var existingRfc = RequestsForChange.FirstOrDefault(r => r.Id == requestForChange.Id);

        if (existingRfc == null)
        {
            // Add the RFC to the list if it doesn't exist (for test purposes)
            requestForChange.SubmittedOn = DateTime.UtcNow; 
            RequestsForChange.Add(requestForChange);
            return Task.FromResult(requestForChange)!;
        }

        existingRfc.ApprovalStatus = requestForChange.ApprovalStatus;
        existingRfc.ApprovedOn = requestForChange.ApprovalStatus == "APPROVED" ? DateTime.UtcNow : requestForChange.ApprovedOn;
        existingRfc.Message = requestForChange.Message;
        
        return Task.FromResult(existingRfc)!;
    }

    public Task<RequestForChange?> GetSingle(Guid id)
    {
        RequestForChange? rfc = RequestsForChange.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(rfc);
    }

    public Task<PaginatedResult<RequestForChange>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending, bool? isPending)
    {
        throw new NotImplementedException();
    }

    public Task<RequestForChange?> UpdateWithoutCommitAsync(RequestForChange requestForChange)
    {
        throw new NotImplementedException();
    }
}
