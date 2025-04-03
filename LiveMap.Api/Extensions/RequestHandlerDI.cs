using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
<<<<<<< HEAD
using RequestForChange = LiveMap.Application.RequestForChange;
using LiveMap.Domain.Models;
=======
using Rfc = LiveMap.Application.RequestForChange;
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956

namespace LiveMap.Api.Extensions;
public static class RequestHandlerDI
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
    {
        services.AddTransient<
            IRequestHandler<
                PointOfInterest.Requests.GetSingleRequest,
                PointOfInterest.Responses.GetSingleResponse>,
            PointOfInterest.Handlers.GetSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                PointOfInterest.Requests.GetMultipleRequest,
                PointOfInterest.Responses.GetMultipleResponse>,
            PointOfInterest.Handlers.GetMultipleHandler>();

        services.AddTransient<
            IRequestHandler<
                Map.Requests.GetSingleRequest,
                Map.Responses.GetSingleResponse>,
            Map.Handlers.GetSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                Map.Requests.GetMultipleRequest,
                Map.Responses.GetMultipleResponse>,
            Map.Handlers.GetMultipleHandler>();

        services.AddTransient<
            IRequestHandler<
                Map.Requests.UpdateBorderRequest,
                Map.Responses.UpdateBorderResponse>,
            Map.Handlers.UpdateBorderHandler>();

<<<<<<< HEAD
        services.AddTransient<IRequestHandler<
            RequestForChange.Requests.GetSingleRequest,
            RequestForChange.Responses.GetSingleResponse>,
            RequestForChange.Handlers.GetSingleHandler>();

        services.AddTransient<IRequestHandler<
            RequestForChange.Requests.GetMultipleRequest,
            RequestForChange.Responses.GetMultipleResponse>,
            RequestForChange.Handlers.GetMultipleHandler>();

        services.AddTransient<IRequestHandler<
            RequestForChange.Requests.CreateSingleRequest,
            RequestForChange.Responses.CreateSingleResponse>,
            RequestForChange.Handlers.CreateRequestHandler>();
=======
        services.AddTransient<
            IRequestHandler<
                Rfc.Requests.CreateSingleRequest,
                Rfc.Responses.CreateSingleResponse>,
            Rfc.Handlers.CreateSingleHandler>();
>>>>>>> 5cacfa20cb2c0d6944e341269a199509cfdcc956

        return services;
    }
}