using LiveMapDashboard.Web.Models.Category;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CategoryListViewModel model)
        {
            return View(model);
        }
    }
}
