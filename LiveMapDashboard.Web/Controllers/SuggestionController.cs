﻿using LiveMapDashboard.Web.Models.Providers;
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

        [HttpPost]
        public async Task<IActionResult> Approve([FromBody] SuggestionFormViewModel viewModel)
        {
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Deny([FromBody] SuggestionFormViewModel viewModel)
        {
            return Ok();
        }
    }
}
