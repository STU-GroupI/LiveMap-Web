namespace LiveMap.Application.RequestForChange.Persistance;
using Domain.Models;

public interface IRequestForChangeRepository
{
    public Task<RequestForChange> CreateAsync(RequestForChange requestForChange);
}