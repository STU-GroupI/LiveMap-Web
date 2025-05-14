using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Models;
using LiveMapDashboard.Web.Models.Dashboard;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LiveMapDashboard.Web.Controllers;

public class DashboardController : Controller
{
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ILogger<DashboardController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/")]
    [HttpGet("/{mapId}")]
    public async Task<IActionResult> Index(
        string? mapId,
        [FromServices] IViewModelProvider<DashboardViewModel> provider)
    {
        if (mapId is not null && Guid.TryParse(mapId, out var id))
        {
            HttpContext.SetSelectedMapId(id);
        }

        Guid? currentMapId = HttpContext.GetCurrentMapId();

        return (mapId, currentMapId) switch
        {
            (null, null) => 
                View(await provider.Provide()),
            
            (null, var current) when current is not null => 
                RedirectToAction("Index", "Dashboard", new { mapId = current }),

            (_, _) => 
                View(await provider.Hydrate(DashboardViewModel.Empty with { MapId = currentMapId}))
        };
    }

    public IActionResult Park()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
