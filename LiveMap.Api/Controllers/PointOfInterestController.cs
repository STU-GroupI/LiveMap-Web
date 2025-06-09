using LiveMap.Api.Models.PointOfInterest;
using LiveMap.Application;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Application.PointOfInterest.Responses;
using LiveMap.Application.PointOfInterest.Validators;
using LiveMap.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/poi")]
public class PointOfInterestController : ControllerBase
{
    /// <summary>
    /// Gets the POI's for a specified map.
    /// </summary>
    /// <param name="id">The id of the specified POI.</param>
    /// <returns>Returns the specified poi. </returns>
    /// <response code="200">Successfully get the poi's.</response>
    /// <response code="404">Poi not found.</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));
        GetSingleResponse response = await handler.Handle(request);

        if (response.PointOfInterest is null)
        {
            return NotFound();
        }

        var poi = response.PointOfInterest;
        poi.Map = null!;
        return Ok(poi);
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
    [ProducesResponseType<PointOfInterest[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMultipleForPark(
        [FromQuery] string mapId,
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IRequestHandler<GetMultipleRequest, GetMultipleResponse> handler)
    {
        var request = new GetMultipleRequest(Guid.Parse(mapId), skip, take);
        var response = await handler.Handle(request);

        return Ok(response.PointsOfInterests);
    }

    /// <summary>
    /// Creates a POI for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> The POI with callback URL </returns>
    /// <response code="201">Response with the created POI</response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, PointOfInterest)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSinglePoiWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        PointOfInterest poi = new()
        {
            Id = Guid.Empty,
            Title = webRequest.Title,
            Description = webRequest.Description,
            Image = webRequest.Image,
            CategoryName = webRequest.CategoryName,
            Coordinate = webRequest.Coordinate,
            IsWheelchairAccessible = webRequest.IsWheelchairAccessible,
            OpeningHours = webRequest.OpeningHours.Select(oh => new OpeningHours
            {
                DayOfWeek = oh.DayOfWeek,
                Start = oh.Start,
                End = oh.End,
                Id = Guid.Empty,
                PoiId = Guid.Empty,
                Poi = null!,
            }).ToList(),

            MapId = Guid.Parse(webRequest.MapId),
            StatusName = "Active",
        };
        var request = new CreateSingleRequest(poi);
        var validationResults = new CreateSingleValidator().Validate(request);

        if (!validationResults.IsValid)
        {
            var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }

        try
        {
            CreateSingleResponse response = await handler.Handle(request);
            if(response.Poi is null)
            {
                throw new ArgumentException("Failed to create POI.");
            }

            return CreatedAtAction(nameof(Get), new { id = response.Poi.Id.ToString() }, response.Poi);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

    /// <summary>
    /// Updates a POI for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> Returns the updated POI </returns>
    /// <response code="200">Response with the updated POI</response>
    /// <response code="500">Something went very wrong</response>
    [HttpPatch("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest>(StatusCodes.Status200OK)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(
        [FromRoute] string id,
        [FromBody] UpdateSinglePoiWebRequest webRequest,
        [FromServices] IRequestHandler<UpdateSingleRequest, UpdateSingleResponse> handler)
    {
        PointOfInterest poi = new()
        {
            Id = Guid.Parse(id),
            Title = webRequest.Title,
            Description = webRequest.Description,
            Image = webRequest.Image,
            CategoryName = webRequest.CategoryName,
            Coordinate = webRequest.Coordinate,
            IsWheelchairAccessible = webRequest.IsWheelchairAccessible,
            OpeningHours = webRequest.OpeningHours.Select(oh => new OpeningHours()
            {
                DayOfWeek = oh.DayOfWeek,
                Start = oh.Start,
                End = oh.End,
                Id = Guid.Empty,
                PoiId = Guid.Empty,
            }).ToList(),

            MapId = Guid.Parse(webRequest.MapId),
            StatusName = "Active",
        };
        var request = new UpdateSingleRequest(poi);
        var validationResults = new UpdateSingleValidator().Validate(request);

        if (!validationResults.IsValid)
        {
            var errorMessages = string.Join(" ", validationResults.Errors.Select(e => e.ErrorMessage));
            return BadRequest(errorMessages);
        }

        try
        {
            UpdateSingleResponse response = await handler.Handle(request);
            if (response.Poi is null)
            {
                return NotFound("Failed to update point of interest.");
            }

            return Ok(response.Poi);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong...");
        }
    }

    /// <summary>
    /// Deletes a POI with the given id
    /// </summary>
    /// <param name="id">The id of the specified POI.</param>
    /// <returns>Returns nothing. </returns>
    /// <response code="204">No Content. </response>
    /// <response code="404">Poi not found.</response>
    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<PointOfInterest>(StatusCodes.Status200OK)]
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
}