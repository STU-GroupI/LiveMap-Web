using LiveMapDashboard.Web.Middleware;

namespace LiveMapDashboard.Web.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this WebApplication? app)
    {
        return app!.UseMiddleware<MapSelectionMiddleware>();
    }
}
