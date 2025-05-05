using LiveMap.Api.Models.Map;
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
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="id">The id of the specified Map.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Poi not found.</response>
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
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="mapId">The map to get the POI's from.</param>
    /// <param name="skip">The amount of items to skip. The base is 0.</param>
    /// <param name="take">The amount of items to take. The base is all.</param>
    /// <returns>Returns the poi's from the given map.</returns>
    /// <response code="200">Successfully get the poi's.</response>
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

    /// <summary>
    /// Creates a map for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> The map with callback URL </returns>
    /// <response code="201">Response with the created map</response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, Map)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSingleMapWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        Map map = new()
        {
            Id = Guid.Empty,
            Name = webRequest.Name,
            Bounds = webRequest.Bounds,
            Area = webRequest.Area,
            ImageUrl = webRequest.ImageUrl
        };
        var request = new CreateSingleRequest(map);

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            return Created("", response.Map);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

    /*[HttpPatch("{id}")]
    public async Task<IActionResult> Patch(
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
    }*/

    /// <summary>
    /// Updates a map for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> Returns the updated map </returns>
    /// <response code="200">Response with the updated map</response>
    /// <response code="500">Something went very wrong</response>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status200OK)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(
        [FromRoute] string id,
        [FromBody] UpdateSingleMapWebRequest webRequest,
        [FromServices] IRequestHandler<UpdateSingleRequest, UpdateSingleResponse> handler)
    {
        Map map = new()
        {
            Id = Guid.Parse(id),
            Name = webRequest.Name,
            Bounds = webRequest.Bounds,
            Area = webRequest.Area,
            ImageUrl = webRequest.ImageUrl
        };
        var request = new UpdateSingleRequest(map);

        try
        {
            UpdateSingleResponse response = await handler.Handle(request);
            return Ok(response.map);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }
}