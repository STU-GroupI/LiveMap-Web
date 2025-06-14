﻿using LiveMapDashboard.Web.Models.Category;
using LiveMapDashboard.Web.Models.Dashboard;
using LiveMapDashboard.Web.Models.Map;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Rfc;

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
                IViewModelProvider<RequestForChangeListViewModel>,
                RequestForChangeListViewModelProvider>()
            .AddTransient<
                IViewModelProvider<MapListViewModel>,
                MapListViewModelProvider>()
            .AddTransient<
                IViewModelProvider<MapCrudformViewModel>,
                MapCrudformViewModelProvider>()
            .AddTransient<
                IViewModelProvider<CategoryCrudFormViewModel>,
                CategoryCrudFormViewModelProvider>()
            .AddTransient<
                IViewModelProvider<RequestForChangeFormViewModel>,
                RequestForChangeFormViewModelProvider>()
            .AddTransient<
                IViewModelProvider<MapSwitcherViewModel>,
                MapSwitcherViewModelProvider>();
    }
}