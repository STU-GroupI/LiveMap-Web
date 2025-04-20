using LiveMap.Persistence.Repositories;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;
using Rfc = LiveMap.Application.RequestForChange;
using Map = LiveMap.Application.Map;
using Category = LiveMap.Application.Category;


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

        services.AddTransient<
            Category.Persistance.ICategoryRepository,
            CategoryRepository>();

        return services;
    }
}
