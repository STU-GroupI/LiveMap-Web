using LiveMap.Api.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterOptions(builder.Configuration);

// Make sure that the connectionstring is automagically picked based on the selected env...
builder.Services.RegisterLiveMapContext(builder.Configuration.GetDatabaseConnectionString());

builder.Services.RegisterRepositories();
builder.Services.RegisterRequestHandlers();
builder.Services.RegisterUnitsOfWork();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }
        await next();
    });
}

// This handles any 500 errors that might occur in the application. Anything we do not catch, this thing will.
app.UseDefaultExceptionHandlingMiddleware();

app.UseCors("CorsPolicy");
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.SeedDatabaseAsync();

app.Run();
