using LiveMap.Application;
using PointOfInterest = LiveMap.Application.PointOfInterest;
using Map = LiveMap.Application.Map;
using Rfc = LiveMap.Application.RequestForChange;
using SuggestedPoi = LiveMap.Application.SuggestedPoi;
using Category = LiveMap.Application.Category;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace LiveMap.Api.Extensions;

public static class RequestValidationDI
{
    public static IServiceCollection RegisterRequestValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Category.Validators.CreateSingleValidator>();
        services.AddValidatorsFromAssemblyContaining<Category.Validators.CategoryValidator>();

        return services;
    }
}
