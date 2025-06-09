using LiveMapDashboard.Web.Models.Rfc;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents;

public class RfcFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(RequestForChangeFormViewModel model)
    {
        return View(model);
    }
}
