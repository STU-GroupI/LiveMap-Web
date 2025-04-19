﻿using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Suggestion;
using LiveMapDashboard.Web.Models.Suggestions;

namespace LiveMapDashboard.Web.Extensions.DI;
public static class ConfigureViewModelProviders
{
    public static IServiceCollection RegisterViewModelProviders(this IServiceCollection services)
    {
        return services.AddTransient<
                IViewModelProvider<PoiCrudformViewModel>,
                PoiCrudformViewModelProvider>()
            .AddTransient<
                IViewModelProvider<PoiCrudformViewModel>,
                PoiCrudformViewModelProvider>()
            .AddTransient<
                IViewModelProvider<SuggestionFormViewModel>,
                SuggestionFormViewModelProvider>()
            .AddTransient<
                IViewModelProvider<PoiSuggestionsViewModel>,
                PoiSuggestionsViewModelProvider>();
    }
}