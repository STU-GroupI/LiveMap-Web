using LiveMap.Application.RequestForChange.UnitsOfWork;
using LiveMap.Persistence.UnitsOfWork.RequestForChange;

namespace LiveMap.Api.Extensions;

public static class UnitOfWorkDI
{
    public static IServiceCollection RegisterUnitsOfWork(this IServiceCollection services)
    {
        return services
            .AddTransient<IApproveRfcUnitOfWork, ApproveRfcUnitOfWork>()
            .AddTransient<IRejectRfcUnitOfWork, RejectRfcUnitOfWork>(); 
    }
}
