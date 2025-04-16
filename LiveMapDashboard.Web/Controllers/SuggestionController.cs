using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Suggestion;
using Microsoft.AspNetCore.Mvc;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[Controller]")]
    public class SuggestionController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(
            [FromRoute] string id, 
            [FromServices] IViewModelProvider<SuggestionFormViewModel> provider)
        {
            var viewModel = await provider.Hydrate(SuggestionFormViewModel.EmptyWithId(id));
            
            return View("SuggestionForm", viewModel);
        }
        
        [HttpPost("Approve")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(SuggestionFormViewModel viewModel)
        {
            return Ok();
        }
        
        [HttpPost("Deny")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(SuggestionFormViewModel viewModel)
        {
            return Ok();
        }
    }
}
