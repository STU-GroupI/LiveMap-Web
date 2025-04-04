﻿using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;

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

        return services;
    }
}