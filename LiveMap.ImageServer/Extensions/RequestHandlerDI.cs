using LiveMap.Application;
using LiveMap.ImageServer.Models.Image;
using Image = LiveMap.Application.Images;

namespace LiveMap.ImageServer.Extensions;

public static class RequestHandlerDI
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
    {
        return services.AddTransient
                <IRequestHandler<Image.Requests.CreateSingleRequest,
                Image.Responses.CreateSingleResponse>,
            Image.Handlers.CreateSingleHandler>();
    }
}
