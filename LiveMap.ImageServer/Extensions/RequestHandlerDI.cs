using LiveMap.Application;
using LiveMap.ImageServer.Models.Image;
using LiveMap.ImageServer.Options;
using Image = LiveMap.Application.Images;

namespace LiveMap.ImageServer.Extensions;

public static class RequestHandlerDI
{
    public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services, ConfigurationManager config)
    {
        // we use ImageService__PublicFacingUrl as the key, so this will bind it correctly
        services.Configure<ImageServiceOptions>(
            config.GetSection(ImageServiceOptions.Position));

        services.AddTransient<
            IRequestHandler<
                Image.Requests.CreateSingleRequest,
                Image.Responses.CreateSingleResponse>,
            Image.Handlers.CreateSingleHandler>();

        return services;
    }
}
