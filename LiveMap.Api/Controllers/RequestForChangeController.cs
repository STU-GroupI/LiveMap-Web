using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Application.RequestForChange.Responses;
using LiveMap.Application;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/rfc")]
public class RequestForChangeController : ControllerBase
{
    /// <summary>
    /// Gets the specified RFC.
    /// </summary>
    /// <param name="id">The id of the specified RFC.</param>
    /// <returns>Returns the specified RFC. </returns>
    /// <response code="200">Successfully get the RFC.</response>
    /// <response code="404">RFC not found.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<RequestForChange>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));
        GetSingleResponse response = await handler.Handle(request);

        if (response.RequestForChange is null)
        {
            return NotFound();
        }

        var rfc = response.RequestForChange;
        return Ok(rfc);
    }

    /// <summary>
    /// Gets the given amount of RFC's from the given points.
    /// </summary>
    /// <param name="skip">The amount of items to skip. The base is 0.</param>
    /// <param name="take">The amount of items to take. The base is all.</param>
    /// <returns>Returns the amount of RFC's from a given point.</returns>
    /// <response code="200">Successfully get the RFC's.</response>
    /// <response code="404">RFC's not found.</response>
    [HttpGet("")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultipleFromPark(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(skip, take);
        var response = await handler.Handle(request);

        return Ok(response.RequestsForChange);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> PostRequestForChange(
        [FromRoute] string id,
        [FromBody] Coordinate[] coordinates,
        [FromServices] IRequestHandler<UpdateBorderRequest, UpdateBorderResponse> handler)
    {
        var response = await handler.Handle(new(Guid.Parse(id), coordinates));

        if (!response.Succeeded)
        {
            return NotFound(id);
        }

        return NoContent();
    }
}

