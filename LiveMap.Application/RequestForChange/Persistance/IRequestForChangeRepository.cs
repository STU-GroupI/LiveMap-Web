namespace LiveMap.Application.RequestForChange.Persistance;
using Domain.Models;

public interface IRequestForChangeRepository
{
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange);
    public Task<ICollection<RequestForChange>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending);
}