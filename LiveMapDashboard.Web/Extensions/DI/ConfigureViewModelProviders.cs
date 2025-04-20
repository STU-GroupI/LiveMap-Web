using LiveMapDashboard.Web.Models.Category;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Rfc;
using LiveMapDashboard.Web.Models.Suggestion;

namespace LiveMapDashboard.Web.Extensions.DI;
public static class ConfigureViewModelProviders
{
    public static IServiceCollection RegisterViewModelProviders(this IServiceCollection services)
    {
        return services
            .AddTransient<
                IViewModelProvider<PoiCrudformViewModel>,
                PoiCrudformViewModelProvider>()
            .AddTransient<
                IViewModelProvider<PoiListViewModel>,
                PoiListViewModelProvider>()
            .AddTransient<IViewModelProvider<CategoryListViewModel>,
                CategoryListViewModelProvider>()
            .AddTransient<
                IViewModelProvider<RequestForChangeViewModel>,
                RequestForChangeViewModelProvider>()
            .AddTransient<
                IViewModelProvider<CategoryCrudFormViewModel>,
                CategoryCrudFormViewModelProvider>();
    }
}