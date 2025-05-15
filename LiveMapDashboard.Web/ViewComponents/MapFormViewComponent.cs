using LiveMapDashboard.Web.Models.Map;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents
{
    public class MapFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MapCrudformViewModel model)
        {
            return View(model);
        }
    }
}
