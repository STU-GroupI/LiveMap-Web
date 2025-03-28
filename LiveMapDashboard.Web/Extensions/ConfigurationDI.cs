using LiveMapDashboard.Web.Options;

namespace LiveMapDashboard.Web.Extensions;
public static class ConfigurationDI
{
    public static IServiceCollection RegisterOptions(this IServiceCollection services, ConfigurationManager config)
    {
        return services.Configure<BackendConfigurationOptions>(options =>
        {
            config.GetSection(BackendConfigurationOptions.Position).Bind(options);
            options.Url = Environment.GetEnvironmentVariable("BACKEND_URL") ?? options.Url;
        });
    }
}
