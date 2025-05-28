using LiveMapDashboard.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Middleware;

public class MapNotFoundExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public MapNotFoundExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (MapNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var problem = new ProblemDetails
            {
                Title = "A custom error occurred",
                Status = 400,
                Detail = ex.Message,
                Type = "https://httpstatuses.com/400"
            };

            context.Response.Redirect("dashboard");
        }
    }
}
