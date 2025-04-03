using LiveMap.Persistence.Repositories;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
<<<<<<< HEAD
using RequestForChange = LiveMap.Application.RequestForChange;
=======
using Rfc = LiveMap.Application.RequestForChange;
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956
using LiveMap.Domain.Models;

namespace LiveMap.Api.Extensions;
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
<<<<<<< HEAD
            RequestForChange.Persistance.IRequestForChangeRepository,
=======
            Rfc.Persistance.IRequestForChangeRepository,
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956
            RequestForChangeRepository>();

        return services;
    }
}
