using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;
using CategoryApp = LiveMap.Application.Category;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace LiveMap.Api.Extensions;
using Domain.Models;

public static class RequestValidationDI
{
    public static IServiceCollection RegisterRequestValidation(this IServiceCollection services)
    {

        return services;
    }
}
