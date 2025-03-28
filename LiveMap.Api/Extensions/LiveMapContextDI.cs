using LiveMap.Persistence;
using LiveMap.Persistence.DataSeeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using static Bogus.DataSets.Name;

namespace LiveMapDashboard.Web.Extensions; 
public static class LiveMapContextDI
{
    public static IServiceCollection RegisterLiveMapContext(this IServiceCollection services, string connectionString)
    {
        return services.AddDbContext<LiveMapContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                options => options.UseNetTopologySuite());
        });
    }

    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<LiveMapContext>();

            // Check if there are any pending migrations
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                // Apply migrations first
                await context.Database.MigrateAsync();
                await DevelopmentSeeder.SeedDatabase(context);
            }
        }
    }
}
