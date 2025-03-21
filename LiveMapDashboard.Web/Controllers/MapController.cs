using LiveMap.Application;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Domain.Models;
using LiveMap.Persistence.Repositories;
using LiveMapDashboard.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;

namespace LiveMapDashboard.Web.Controllers;

[ApiController]
[Route("api/map")]
public class MapController : ControllerBase
{
    /// <summary>
    /// Gets the requested map.
    /// </summary>
    /// <param name="id">The id of the specified Map.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the map.</response>
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
    /// Gets all maps.
    /// </summary>
    /// <param name="skip">The amount of items to skip. The base is 0.</param>
    /// <param name="take">The amount of items to take. The base is all.</param>
    /// <returns>Returns all maps within the set bounds.</returns>
    /// <response code="200">Successfully get the maps.</response>
    /// <response code="404">Map not found.</response>
    [HttpGet("")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultiple(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(skip, take);
        var response = await handler.Handle(request);

        return Ok(response.Maps);
    }
}