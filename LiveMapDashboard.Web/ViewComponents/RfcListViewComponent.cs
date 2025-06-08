using LiveMapDashboard.Web.Models.Rfc;

namespace LiveMapDashboard.Web.ViewComponents {
    using Microsoft.AspNetCore.Mvc;

    public class RfcListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(RequestForChangeListViewModel model)
        {
            return View(model);
        }
    }
}