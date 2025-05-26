using LiveMap.Application.Infrastructure.Services;
using LiveMapDashboard.Web.Extensions;

namespace LiveMapDashboard.Web.Middleware;

public class MapSelectionMiddleware
{
    private readonly RequestDelegate _next;

    public MapSelectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMapService mapService)
    {
        if (!HttpMethods.IsGet(context.Request.Method))
        {
            await _next(context);
            return;
        }

        if (context.ContainsSelectedMapId())
        {
            await _next(context);
            return;
        }

        var serviceResult = await mapService.Get(0, 1);
        if (serviceResult.IsSuccess && serviceResult.Value is { TotalCount: > 0 })
        {
            var firstMapId = serviceResult.Value.Items[0].Id;
            context.SetSelectedMapId(firstMapId);
        }

        await _next(context);
    }
}
