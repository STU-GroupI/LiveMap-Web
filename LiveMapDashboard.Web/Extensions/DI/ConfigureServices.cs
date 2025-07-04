﻿
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

                .AddTransient<IImageServerHttpService, ImageServerHttpService>()
                .AddTransient<IImageService, ImageHttpService>()

                .AddTransient<IPointOfInterestService, PointOfInterestHttpService>()
                .AddTransient<ICategoryService, CategoryHttpService>()
                .AddTransient<IMapService, MapHttpService>()
                .AddTransient<IRequestForChangeService, RequestForChangeHttpService>()
                .AddTransient<ISuggestedPointOfInterestService, SuggestedPointOfInterestHttpService>();
        }
    }
}
