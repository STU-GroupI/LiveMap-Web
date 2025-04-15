using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers;

public class PoiController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}