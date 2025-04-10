
using LiveMap.Application.Infrastructure.Services;
using LiveMap.Infrastructure.Services;

namespace LiveMapDashboard.Web.Extensions.DI
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IBackendApiHttpService, BackendApiHttpService>()
                .AddTransient<IPointOfInterestService, PointOfInterestHttpService>();
        }
    }
}
