using LiveMapDashboard.Web.Models.Category;
using LiveMapDashboard.Web.Models.Poi;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents;
public class CategoryFormViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(CategoryCrudFormViewModel model)
    {
        return View(model);
    }
}
