using Models = LiveMap.Domain.Models;

namespace LiveMap.Application.RequestForChange.Persistance;
using Domain.Models;

public interface IRequestForChangeRepository
{
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange);
    public Task<RequestForChange> UpdateAsync(RequestForChange requestForChange);
    public Task<Models.RequestForChange?> GetSingle(Guid id);
}