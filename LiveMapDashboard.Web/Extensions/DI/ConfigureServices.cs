using LiveMapDashboard.Web.Services.Communication;

namespace LiveMapDashboard.Web.Extensions.DI
{
    public static class ConfigureServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IBackendApiService, BackendApiHttpService>()
                .AddTransient<IPointOfInterestService, PointOfInterestHttpService>();
        }
    }
}
