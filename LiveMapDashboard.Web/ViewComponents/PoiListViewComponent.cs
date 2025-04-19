using LiveMapDashboard.Web.Models.Poi;

namespace LiveMapDashboard.Web.ViewComponents {
    using Microsoft.AspNetCore.Mvc;

    public class PoiListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PoiListViewModel model)
        {
            return View(model);
        }
    }
}