using LiveMapDashboard.Web.Models.Map;

namespace LiveMapDashboard.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class MapListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(MapListViewModel model)
        {
            return View(model);
        }
    }
}
