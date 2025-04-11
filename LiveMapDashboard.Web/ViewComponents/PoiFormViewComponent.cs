using LiveMapDashboard.Web.Models.Poi;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents
{
    public class PoiFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PoiCrudformViewModel model)
        {
            return View(model);
        }
    }
}