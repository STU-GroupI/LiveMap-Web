using LiveMap.Application.Persistance;
using LiveMap.Application.RequestForChange.Requests;

namespace LiveMap.Application.RequestForChange.UnitsOfWork;

public interface IApproveRfcUnitOfWork : IUnitOfWork<ApprovalRequest, bool>
{
}