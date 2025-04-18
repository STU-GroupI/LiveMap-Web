using LiveMap.Application;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Domain.Models;
using LiveMap.Domain.Pagination;
using LiveMapDashboard.Web.Models.Providers;
using LiveMapDashboard.Web.Models.Rfc;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class RFCController : Controller
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(
            [FromRoute] string id,
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromQuery] bool? ascending,
            [FromServices] IViewModelProvider<RequestForChangeViewModel> provider)
        {
            var viewModel = await provider.Hydrate(new RequestForChangeViewModel(Guid.Parse(id), skip, take, ascending, PaginatedResult<RequestForChange>.Default));
            return View(viewModel);
        }
    }
}
