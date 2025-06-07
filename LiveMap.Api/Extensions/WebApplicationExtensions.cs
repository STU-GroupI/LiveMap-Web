using Microsoft.AspNetCore.Diagnostics;

namespace LiveMap.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDefaultExceptionHandlingMiddleware(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var error = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Internal Server Error",
                });
            });
        });
        return app;
    }
}
