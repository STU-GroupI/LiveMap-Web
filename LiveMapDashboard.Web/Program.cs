using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Extensions.DI;
using System.Globalization;
using LiveMapDashboard.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

// Does the spooky config management injection hmmm yes
builder.Services.RegisterOptions(builder.Configuration);
builder.Services.ConfigureHttpClients();
builder.Services.RegisterServices();
builder.Services.RegisterViewModelProviders();

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiddleware<MapNotFoundExceptionHandlingMiddleware>();



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCustomMiddleware();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();