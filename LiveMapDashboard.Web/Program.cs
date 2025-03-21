
using Microsoft.EntityFrameworkCore;
using LiveMapDashboard.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Make sure that the connectionstring is automagically picked based on the selected env...
builder.Services.RegisterLiveMapContext(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.RegisterRepositories();
builder.Services.RegisterRequestHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

// Does not have to be awaited as far as we are concerned. It can just run seperately as a job.
await app.SeedDatabase();

app.UseSwagger();
app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
{
    options.SwaggerEndpoint("api/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();