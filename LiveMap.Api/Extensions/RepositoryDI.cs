using LiveMap.Persistence.Repositories;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using RequestForChange = LiveMap.Application.RequestForChange;
using LiveMap.Domain.Models;

namespace LiveMapDashboard.Web.Extensions;
public static class RepositoryDI
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<
            PointOfInterest.Persistance.IPointOfInterestRepository,
            PointOfInterestRepository>();

        services.AddTransient<
            Map.Persistance.IMapRepository,
            MapRepository>();

        services.AddTransient<
            RequestForChange.Persistance.IRequestForChangeRepository,
            RequestForChangeRepository>();

        return services;
    }
}
