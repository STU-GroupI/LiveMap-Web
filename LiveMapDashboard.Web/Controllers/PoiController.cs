using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

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
            [FromServices] IHttpClientFactory httpClientFactory,
            PoiCrudformViewModel viewModel,
            string action)
        {
            var client = httpClientFactory.CreateBackendClient();

            if (client is null)
            {
                ModelState.AddModelError(
                    "InternalError",
                    "The backend service is currently unavailable. Please try again later.");
                return View(viewModel);
            }

            try
            {
                var result = await client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(action),
                });

                if (result.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Your request was successfully processed!";
                }
                switch (result.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        ModelState.AddModelError(
                            "BadRequest",
                            "The request was invalid. Please check the data you submitted.");
                        break;
                    case HttpStatusCode.Unauthorized:
                        ModelState.AddModelError(
                            "Unauthorized",
                            "You are not authorized to perform this action.");
                        break;
                    case HttpStatusCode.ServiceUnavailable:
                        ModelState.AddModelError(
                            "ServiceUnavailable",
                            "The service is currently unavailable. Please try again later.");
                        break;
                    default:
                        ModelState.AddModelError(
                            "GeneralError",
                            "An unexpected error occurred. Please try again.");
                        break;
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(
                    "RequestError",
                    "An error occurred while contacting the backend service. Please try again later.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    "UnexpectedError",
                    "An unexpected error occurred. Please try again later.");
            }

            return View(viewModel);
        }
    }
}
