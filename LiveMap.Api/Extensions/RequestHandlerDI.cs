using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;
using Category = LiveMap.Application.Category;

namespace LiveMap.Api.Extensions;
public static class RequestHandlerDI
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
    {
        foreach (var handlerTypeInformation in Helpers.ScanHandlersFromAssembly())
        {
            services.AddTransient(
                handlerTypeInformation.Interface, 
                handlerTypeInformation.Implementation);
        }

        return services;
    }
}
