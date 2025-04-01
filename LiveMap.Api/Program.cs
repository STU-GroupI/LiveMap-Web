
using LiveMapDashboard.Web.Extensions;
using System.Threading.Tasks;

namespace LiveMap.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Make sure that the connectionstring is automagically picked based on the selected env...
            builder.Services.RegisterLiveMapContext(
                builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Connectionstring not set"));

            builder.Services.RegisterRepositories();
            builder.Services.RegisterRequestHandlers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
