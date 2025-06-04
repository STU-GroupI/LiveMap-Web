using LiveMap.Application.Infrastructure.Services;
using LiveMap.Domain.Models;
using LiveMapDashboard.Web.Exceptions;
using LiveMapDashboard.Web.Extensions;
using LiveMapDashboard.Web.Extensions.Mappers;
using LiveMapDashboard.Web.Models.Poi;
using LiveMapDashboard.Web.Models.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LiveMapDashboard.Web.Controllers;

public class PoiController : Controller
{
    [HttpGet("{mapId}/[controller]")]
    public async Task<IActionResult> Index(
        [FromRoute] string mapId,
        [FromServices] IViewModelProvider<PoiListViewModel> provider)
    {
        var viewModel = new PoiListViewModel()
        {
            MapId = Guid.Parse(mapId),
        };
        try
        {
            viewModel = await provider.Hydrate(viewModel);
        }
        catch(MapNotFoundException)
        {
            return View(viewModel);
        }
        
        return View(viewModel);
    }

    [HttpGet("{mapId}/[controller]/form/{id}")]
    public async Task<IActionResult> PoiUpdateForm(
        [FromRoute] string id,
        [FromRoute] string mapId,
        [FromServices] IViewModelProvider<PoiCrudformViewModel> provider)
    {
        if (!Guid.TryParse(id, out var poiId))
        {
            var back = HttpContext?.Request?.Headers?.ContainsKey("Referer") ?? false;

            string backValue = back 
                ? HttpContext!.Request.Headers["Referer"].ToString() 
                : string.Empty;
            return back 
                ? Redirect(backValue) 
                : RedirectToAction(
                    nameof(Index),
                    "poi",  
                    new { mapId = HttpContext!.GetCurrentMapId()?.ToString() ?? string.Empty });
        }

        var viewModel = await provider.Hydrate(PoiCrudformViewModel.Empty with
        {
            Id = id,
            MapId = mapId
        });

        return View("PoiForm", viewModel);
    }

    [HttpGet("{mapId}/[controller]/form")]
    public async Task<IActionResult> PoiCreateForm(
        [FromRoute] string mapId,
        [FromServices] IViewModelProvider<PoiCrudformViewModel> provider)
    {
        var viewModel = await provider.Hydrate(PoiCrudformViewModel.Empty with
        {
            Id = null,
            MapId = mapId
        });
        return View("PoiForm", viewModel);
    }

    [HttpPost("[controller]/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        [FromServices] IPointOfInterestService service,
        [FromServices] IViewModelProvider<PoiCrudformViewModel> provider,
        PoiCrudformViewModel viewModel)
    {
        var poiId = viewModel.Id is not null
            ? Guid.Parse(viewModel.Id)
            : Guid.Empty;
        
        if (poiId == Guid.Empty)
        {
            ViewData["ErrorMessage"] = "The point of interest was not found.";
            return View("PoiForm", await provider.Provide());
        }
        
        var result = await service.Delete(poiId);
        
        if (result.IsSuccess)
        {
            ViewData["SuccessMessage"] = "The point of interest and its contents where successfully deleted!";
            return RedirectToAction(
                nameof(Index),
                "poi",
                new { mapId = HttpContext!.GetCurrentMapId()?.ToString() ?? string.Empty });
        }
        
        ViewData["ErrorMessage"] = result.StatusCode switch
        {
            HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
            HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
            HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
            _ => "Something went wrong while trying to contact the application. Please try again later"
        };
        
        return View("PoiForm", viewModel);
    }

    [HttpPost]
    [Route("[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        [FromServices] IPointOfInterestService poiService,
        [FromServices] IImageService imageService,
        [FromServices] IViewModelProvider<PoiCrudformViewModel> provider,
        PoiCrudformViewModel viewModel,
        string action)
    {
        if (!ModelState.IsValid)
        {
            return View("PoiForm", await provider.Hydrate(viewModel));
        }

        // Image
        if (viewModel.ImageFile is IFormFile imageFile)
        {
            var imageResult = await imageService.Create(new(ToImage(imageFile)));
            viewModel = viewModel with { Image = imageResult.Value };
        }

        // POI
        var poi = viewModel.ToDomainPointOfInterest();
        var isNewPoi = poi.Id == Guid.Empty || poi.Id.ToString() == string.Empty;

        var result = isNewPoi
            ? await poiService.CreateSingle(poi)
            : await poiService.UpdateSingle(poi);

        if (isNewPoi)
        {
            return RedirectToAction(
                nameof(Index),
                "poi",
                new { mapId = viewModel.MapId });
        }
        
        if (result.IsSuccess)
        {
            ViewData["SuccessMessage"] = "Your request was successfully processed!";
        }
        else
        {
            ViewData["ErrorMessage"] = result.StatusCode switch
            {
                HttpStatusCode.BadRequest => "The submitted data was invalid. Please check the data you submitted.",
                HttpStatusCode.Unauthorized => "You are not authorized to perform this action.",
                HttpStatusCode.ServiceUnavailable => "The application unavailable. Please try again later.",
                _ => "Something went wrong while trying to contact the application. Please try again later"
            };
        }
        
        return View("PoiForm", await provider.Hydrate(viewModel));
    }

    public static string ToImage(IFormFile imageFile)
    {
        //using var memoryStream = new MemoryStream();
        //imageFile.CopyTo(memoryStream);
        //byte[] imageBytes = memoryStream.ToArray();
        //return Convert.ToBase64String(imageBytes);

        using var memoryStream = new MemoryStream();
        imageFile.CopyTo(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();

        // Get content type (e.g., image/png, image/jpeg)
        string contentType = imageFile.ContentType;

        // Return data URI
        return $"data:{contentType};base64,{Convert.ToBase64String(imageBytes)}";
    }
}
