using LiveMapDashboard.Web.Options;

namespace LiveMapDashboard.Web.Extensions.DI;
public static class ConfigurationDI
{
    public static IServiceCollection RegisterOptions(this IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<BackendConfigurationOptions>(options =>
        {
            config.GetSection(BackendConfigurationOptions.Position).Bind(options);
            options.Url = Environment.GetEnvironmentVariable("BACKEND_URL") ?? options.Url;
        });

        services.Configure<ImageConfigurationOptions>(options =>
        {
            config.GetSection(ImageConfigurationOptions.Position).Bind(options);
            options.Url = Environment.GetEnvironmentVariable("IMAGE_SERVER_URL") ?? options.Url;
        });

        return services;
    }
}
