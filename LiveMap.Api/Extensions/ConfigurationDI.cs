using LiveMap.Api.Options;

namespace LiveMap.Api.Extensions;

public static class ConfigurationDI
{
    private const string _DATABASE_CON = "DATABASE_CON";
    public static IServiceCollection RegisterOptions(this IServiceCollection services, ConfigurationManager config)
    {
        return services.Configure<ConnectionStringOptions>(options =>
        {
            config.GetSection(ConnectionStringOptions.Position).Bind(options);
            options.DefaultConnection = Environment.GetEnvironmentVariable(_DATABASE_CON) ?? options.DefaultConnection;
        });
    }

    // The reason we have this one as well is because we want to access the options for database connection strings before
    // we have a build app and with that a working service container. This is a justifiable work-around for the issue.
    public static string GetDatabaseConnectionString(this ConfigurationManager manager)
    {
        var options = new ConnectionStringOptions();
        manager.GetSection(ConnectionStringOptions.Position).Bind(options);
        options.DefaultConnection = Environment.GetEnvironmentVariable(_DATABASE_CON) 
            ?? options.DefaultConnection 
            ?? throw new ArgumentNullException("No database connection string was provided");

        return options.DefaultConnection;
    }
}
