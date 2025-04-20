using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;
using Category = LiveMap.Application.Category;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Handlers;

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
                Category.Requests.UpdateSingleRequest,
                Category.Responses.UpdateSingleResponse>,
            Category.Handlers.UpdateSingleHandler>();
        
        services.AddTransient<
            IRequestHandler<
                Category.Requests.DeleteSingleRequest,
                Category.Responses.DeleteSingleResponse>,
            Category.Handlers.DeleteSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                PointOfInterest.Requests.CreateSingleRequest,
                PointOfInterest.Responses.CreateSingleResponse>,
            PointOfInterest.Handlers.CreateSingleHandler>();

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

        services.AddTransient<
            IRequestHandler<
                Rfc.Requests.CreateSingleRequest,
                Rfc.Responses.CreateSingleResponse>,
            Rfc.Handlers.CreateSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                SuggestedPoi.Requests.CreateSingleRequest,
                SuggestedPoi.Responses.CreateSingleResponse>,
            SuggestedPoi.Handlers.CreateSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                GetMultipleRequest,
                GetMultipleResponse>,
            GetMultipleHandler>();

        services.AddTransient<
            IRequestHandler<                
                Category.Requests.GetSingleRequest,
                Category.Responses.GetSingleResponse>,
            Category.Handlers.GetSingleHandler>();

        services.AddTransient<
            IRequestHandler<
                Category.Requests.GetMultipleRequest,
                Category.Responses.GetMultipleResponse>,
            Category.Handlers.GetMultipleHandler>();

        return services;
    }
}