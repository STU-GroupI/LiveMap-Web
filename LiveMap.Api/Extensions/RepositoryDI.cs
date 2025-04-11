using LiveMap.Persistence.Repositories;
using Map = LiveMap.Application.Map;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;

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
            Rfc.Persistance.IRequestForChangeRepository,
            RequestForChangeRepository>();

        services.AddTransient<
            SuggestedPoi.Persistanc.ISuggestedPointOfInterestRepository,
            SuggestedPointOfInterestRepository>();

        return services;
    }
}
