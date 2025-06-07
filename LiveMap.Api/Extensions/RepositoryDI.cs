using LiveMap.Persistence.Repositories;
using Category = LiveMap.Application.Category;
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
            Category.Persistance.ICategoryRepository,
            CategoryRepository>();

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
