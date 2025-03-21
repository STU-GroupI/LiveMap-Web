using LiveMap.Persistence.Repositories;
using PointOfInterest = LiveMap.Application.PointOfInterest;

namespace LiveMapDashboard.Web.Extensions;
public static class RepositoryDI
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<
            PointOfInterest.Persistance.IPointOfInterestRepository,
            PointOfInterestRepository>();

        return services;
    }
}
