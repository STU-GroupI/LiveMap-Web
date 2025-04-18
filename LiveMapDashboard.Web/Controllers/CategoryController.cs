using LiveMapDashboard.Web.Models.Categories;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class CategoriesController : Controller
    {
        /// <summary>
        /// Displays the list of all categories.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromServices] IViewModelProvider<IEnumerable<CategoryViewModel>> provider)
        {
            // Hydrate with an empty input; provider will load all categories from the data store.
            var categories = await provider.Hydrate(Enumerable.Empty<CategoryViewModel>());
            return View(categories);
        }
    }
}
