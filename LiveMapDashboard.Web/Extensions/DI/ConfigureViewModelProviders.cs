using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;

namespace LiveMapDashboard.Web.Extensions.DI;
public static class ConfigureViewModelProviders
{
    public static IServiceCollection RegisterViewModelProviders(this IServiceCollection services)
    {
        return services
            .AddTransient<
                IViewModelProvider<PoiCrudformViewModel>, 
                PoiCrudformViewModelProvider>();
    }
}