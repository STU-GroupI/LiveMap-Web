using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Models.Poi;
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
            [FromServices] IHttpClientFactory httpClientFactory,
            PoiCrudformViewModel viewModel,
            string action)
        {
            var client = httpClientFactory.CreateBackendClient();

            if (client is null)
            {
                ViewData["ErrorMessage"] = "The backend service is currently unavailable. Please try again later.";
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
                    ViewData["Success"] = "Your request was successfully processed!";
                }

                ViewData["ErrorMessage"] = result.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                    _ => "An unexpected error occurred. Please try again."
                };
            }
            catch (HttpRequestException ex)
            {
                ViewData["ErrorMessage"] = "An error occurred while contacting the application. Please try again later.";

                return View("index", viewModel with
                {
                    Categories = []
                });
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";

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
