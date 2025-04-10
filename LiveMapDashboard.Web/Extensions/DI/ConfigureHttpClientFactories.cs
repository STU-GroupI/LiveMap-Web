using LiveMap.Infrastructure.Extensions;
using LiveMapDashboard.Web.Options;
using Microsoft.Extensions.Options;

namespace LiveMapDashboard.Web.Extensions.DI
{
    public static class ConfigureHttpClientFactories
    {
        public static IServiceCollection ConfigureHttpClients(
            this IServiceCollection services)
        {
            services.AddHttpClient();
            services.RegisterBackendHttpClient();

            return services;
        }

        public static IServiceCollection RegisterBackendHttpClient(
            this IServiceCollection services)
        {
            services.AddHttpClient(
                IHttpClientFactoryExtensions.BackendClientName, 
                (serviceProvider, client) =>
                {
                    var options = serviceProvider
                        .GetRequiredService<IOptions<BackendConfigurationOptions>>().Value;

                    client.BaseAddress = new Uri($"{options.Url}/{options.Api}");
                    //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
                });

            return services;
        }
    }
}
