namespace LiveMap.Application.RequestForChange.Persistance;
using Domain.Models;
using LiveMap.Domain.Pagination;

public interface IRequestForChangeRepository
{
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange);
    public Task<PaginatedResult<RequestForChange>> GetMultiple(Guid parkId, int? skip, int? take, bool? ascending);
}