using Microsoft.AspNetCore.Mvc;
using LiveMap.Domain.Models;
using LiveMap.Application;
using LiveMapDashboard.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;
using LiveMap.Application.RequestForChange.Handlers;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;

namespace LiveMapDashboard.Web.Controllers;

[ApiController]
[Route("api/rfc")]
public class RequestForChangeController : ControllerBase
{
    /// <summary>
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Poi not found.</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(
        [FromBody] RequestForChange webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        var request = new CreateSingleRequest(webRequest);

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            return Created("", response.Rfc);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }
}