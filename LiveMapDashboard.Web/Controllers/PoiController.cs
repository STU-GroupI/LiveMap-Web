using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Services.Communication;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers
{
    [Route("[controller]")]
    public class PoiController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = PoiCrudformViewModel.Empty with
            {
                Categories = [
                    new() { CategoryName = "Food" }, 
                    new() { CategoryName = "Entertainment" }, 
                    new() { CategoryName = "Park" }, 
                    new() { CategoryName = "Museum" }],

                ParkId = Guid.NewGuid().ToString(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(
            [FromServices] IPointOfInterestService service,
            PoiCrudformViewModel viewModel,
            string action)
        {
            if (!ModelState.IsValid)
            {
                return View("index", viewModel with
                {
                    Categories = [
                        new() { CategoryName = "Food" },
                        new() { CategoryName = "Entertainment" },
                        new() { CategoryName = "Park" },
                        new() { CategoryName = "Museum" }]
                });
            }
            
            var result = await service.CreateSingle(viewModel.ToDomainPointOfInterest());

            if (result.IsSuccess)
            {
                ViewData["Success"] = "Your request was successfully processed!";
                return View("index", viewModel with
                {
                    Categories = [
                    new() { CategoryName = "Food" },
                    new() { CategoryName = "Entertainment" },
                    new() { CategoryName = "Park" },
                    new() { CategoryName = "Museum" }]
                });
            }

            if (result.StatusCode is null)
            {
                ViewData["ErrorMessage"] = "Something went wrong while trying to contact the application. Please try again later";
            }
            else
            {
                ViewData["ErrorMessage"] = result.StatusCode switch
                {
                    HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                    HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                    HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                    _ => "An unexpected error occurred. Please try again."
                };
            }
            
            return View("index", viewModel with
            {
                Categories = [
                    new() { CategoryName = "Food" },
                    new() { CategoryName = "Entertainment" },
                    new() { CategoryName = "Park" },
                    new() { CategoryName = "Museum" }]
            });
        }
    }
}
