
using LiveMap.ImageServer.Extensions;

namespace LiveMap.ImageServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set WebRootPath explicitly if not detected or present
            if (string.IsNullOrEmpty(builder.Environment.WebRootPath))
            {
                var currentDir = Directory.GetCurrentDirectory();
                var webRoot = Path.Combine(currentDir, "wwwroot");
                builder.Environment.WebRootPath = webRoot;
            }
            
            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            // Add services to the container.
            builder.Services.RegisterRequestHandlers();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            
            app.UseCors("AllowAll");
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthorization();


            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
