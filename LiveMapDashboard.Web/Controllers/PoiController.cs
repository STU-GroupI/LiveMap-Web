using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class PoiController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = PoiCrudformViewModel.Empty;
            return View(viewModel);
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            [FromServices] IBackendApiRequestService service,
            PoiCrudformViewModel viewModel,
            string action)
        {
            try
            {
                var result = await service.SendRequest<PointOfInterest>(new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(action),
                });

                if (result.IsSuccess)
                {
                    TempData["Success"] = "Your request was successfully processed!";
                }

                TempData["ErrorMessage"] = result.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                    _ => "An unexpected error occurred. Please try again."
                };
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";

                return View("index", viewModel with
                {
                    Categories = []
                });
            }

            return View("index", viewModel with
            {
                Categories = []
            });
        }
    }
}
