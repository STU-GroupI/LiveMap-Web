using LiveMap.Application.Persistance;
using LiveMap.Application.RequestForChange.Requests;

namespace LiveMap.Application.RequestForChange.UnitsOfWork;

public interface IRejectRfcUnitOfWork : IUnitOfWork<RejectRfcRequest, bool>
{
}