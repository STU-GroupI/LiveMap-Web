using Microsoft.AspNetCore.Mvc;
using LiveMap.Domain.Models;
using System.Net.Mime;
using LiveMap.Application.SuggestedPoi.Requests;
using LiveMap.Application.SuggestedPoi.Responses;
using LiveMap.Api.Models.SuggestedPoi;
using LiveMap.Application;

namespace LiveMap.Api.Controllers;

[ApiController]
[Route("api/rfc/poisuggestion")]
public class SuggestedPoiController : ControllerBase
{
    /// <summary>
    /// Creates an RFC for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpGet("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<SuggestedPointOfInterest>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(
        [FromRoute] string id,
        [FromServices] IRequestHandler<GetSingleRequest, GetSingleResponse> handler)
    {
        var request = new GetSingleRequest(Guid.Parse(id));

        GetSingleResponse response = await handler.Handle(request);

        if (response.SuggestedPoi is null)
        {
            return NotFound();
        }

        return Ok(response.SuggestedPoi);
    }

    /// <summary>
    /// Creates an RFC for the given request data
    /// </summary>
    /// <param name="request">The given request.</param>
    /// <returns> The RFC with callback URL </returns>
    /// <response code="201">Response with the created </response>
    /// <response code="500">Something went very wrong</response>
    [HttpPost()]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType<(string, RequestForChange)>(StatusCodes.Status201Created)]
    [ProducesResponseType<(int, object)>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post(
        [FromBody] CreateSinglePoiSuggestionWebRequest webRequest,
        [FromServices] IRequestHandler<CreateSingleRequest, CreateSingleResponse> handler)
    {
        var poi = new SuggestedPointOfInterest()
        {
            Title = webRequest.Title,
            Description = webRequest.Description,
            CategoryName = webRequest.Category,
            Coordinate = webRequest.Coordinate,
            IsWheelchairAccessible = webRequest.isWheelchairAccessible,
            MapId = webRequest.MapId,
            Id = default
        };

        var request = new CreateSingleRequest(poi);

        CreateSingleResponse response = await handler.Handle(request);

        return CreatedAtAction(nameof(Get), new { id = response.ToString() }, response.SuggestedPoi);
    }
}