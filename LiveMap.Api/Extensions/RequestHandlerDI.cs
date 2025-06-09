using LiveMap.Application;

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
