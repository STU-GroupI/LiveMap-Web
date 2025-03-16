using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;

namespace LiveMapDashboard.Web.Extensions;
public static class RequestHandlerDI
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
    {
        services.AddTransient<IRequestHandler<
            PointOfInterest.Requests.GetSingleRequest,
            PointOfInterest.Responses.GetSingleResponse>,
            PointOfInterest.Handlers.GetSingleHandler>();

        services.AddTransient<IRequestHandler<
            PointOfInterest.Requests.GetPagedRequest,
            PointOfInterest.Responses.GetPagedResponse>,
            PointOfInterest.Handlers.GetPagedHandler>();

        return services;
    }
}