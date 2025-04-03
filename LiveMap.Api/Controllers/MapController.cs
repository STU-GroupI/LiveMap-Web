using LiveMap.Application;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/map")]
public class MapController : ControllerBase
{
    /// <summary>
    /// Gets the specified Map.
    /// </summary>
    /// <param name="id">The id of the specified Map.</param>
    /// <returns>Returns the specified Map. </returns>
    /// <response code="200">Successfully get the Map.</response>
    /// <response code="404">Map not found.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));
        GetSingleResponse response = await handler.Handle(request);

        if (response.Map is null)
        {
            return NotFound();
        }

        var map = response.Map;
        return Ok(map);
    }

    /// <summary>
    /// Gets the given amount of Maps from the given points.
    /// </summary>
    /// <param name="skip">The amount of items to skip. The base is 0.</param>
    /// <param name="take">The amount of items to take. The base is all.</param>
    /// <returns>Returns the amount of maps from a given point.</returns>
    /// <response code="200">Successfully get the Maps.</response>
    /// <response code="404">Maps not found.</response>
    [HttpGet("")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultipleForPark(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(skip, take);
        var response = await handler.Handle(request);

        return Ok(response.Maps);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PostForPark(
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