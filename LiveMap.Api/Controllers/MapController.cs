using LiveMap.Api.Models.Map;
using LiveMap.Application;
using LiveMap.Application.Map.Requests;
using LiveMap.Application.Map.Responses;
using LiveMap.Application.Map.Validators;
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

        return Ok(response.Result);
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
            ImageUrl = webRequest.ImageUrl,
        };
        var request = new CreateSingleRequest(map);
        var validationResults = new CreateSingleValidator().Validate(request);

        try
        {
            if (!validationResults.IsValid)
            {
                var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errorMessages);
            }

            CreateSingleResponse response = await handler.Handle(request);
            if (response.Map is null)
            {
                throw new ArgumentException("Failed to create map.");
            }

            return CreatedAtAction(nameof(Get), new { id = response.Map.Id.ToString() }, response.Map);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Something went wrong...");
        }
    }

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
        var validationResults = new UpdateSingleValidator().Validate(request);

        try
        {
            if (!validationResults.IsValid)
            {
                var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errorMessages);
            }

            UpdateSingleResponse response = await handler.Handle(request);
            if(response.Map is null)
            {
                throw new ArgumentException("Failed to update map.");
            }

            return Ok(response.Map);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message ?? "Something went wrong...");
        }
    }

    /// <summary>
    /// Deletes a map with the given id
    /// </summary>
    /// <param name="id">The id of the specified map.</param>
    /// <returns>Returns nothing. </returns>
    /// <response code="204">No Content. </response>
    /// <response code="404">Map not found.</response>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] string id,
        [FromServices] IRequestHandler<DeleteSingleRequest> handler)
    {
        var request = new DeleteSingleRequest(Guid.Parse(id));
        var result = await handler.Handle(request);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }


    /// <summary>
    /// Returns a map that is closest to the given latitude and longitude coordinates.
    /// </summary>
    /// <param name="latitude">Latitude Coordinate of request</param>
    /// <param name="longitude">Longitude Coordinate of request</param>
    /// <returns>Either a map which is in line with the given coordinates, or a 404</returns>
    [HttpGet("closest")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<Map>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClosest(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromServices] IRequestHandler<GetClosestRequest, GetClosestResponse> handler)
    {
        var request = new GetClosestRequest(latitude, longitude);

        GetClosestResponse response = await handler.Handle(request);
        if (response.Map is null)
        {
            return NotFound();
        }

        return Ok(response.Map);
    }
}