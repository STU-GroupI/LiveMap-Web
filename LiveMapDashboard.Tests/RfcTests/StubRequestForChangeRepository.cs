using LiveMap.Application.RequestForChange.Persistance;
using LiveMap.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Status = ApprovalStatus.PENDING,
            PoiId = requestForChange.PoiId,
            SuggestedPoiId = requestForChange.SuggestedPoiId,
            SubmittedOn = DateTime.UtcNow,
        });
    }
}
