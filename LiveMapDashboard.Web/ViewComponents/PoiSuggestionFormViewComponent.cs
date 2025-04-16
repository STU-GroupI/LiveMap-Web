using LiveMapDashboard.Web.Models.Suggestion;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents
{
    public class PoiSuggestionFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SuggestionFormViewModel model)
        {
            return View(model);
        }
    }
}