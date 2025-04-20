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

    public Task<PaginatedResult<RequestForChange>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending)
    {
        throw new NotImplementedException();
    }
}
